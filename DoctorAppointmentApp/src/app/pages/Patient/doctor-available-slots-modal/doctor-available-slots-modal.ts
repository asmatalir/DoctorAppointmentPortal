import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { DoctorsService } from '../../../core/services/doctors-service';
import { Router } from '@angular/router';
import { AppointmentRequestsModel } from '../../../core/models/AppointmentRequestsModel';
import { Action } from 'rxjs/internal/scheduler/Action';
import { AppointmentRequestService } from '../../../core/services/appointment-request-service';

@Component({
  selector: 'app-doctor-available-slots-modal',
  standalone : false,
  templateUrl: './doctor-available-slots-modal.html',
  styleUrls: ['./doctor-available-slots-modal.scss']
})
export class DoctorAvailableSlotsModal implements OnInit {
  @Input() doctorId!: number;
  @Input() SpecializationId!: number;
  @Input() doctorName!: string;
  @Input() doctorEmail!: string;
  @Input() patientName!: string;
  @Input() patientEmail!: string;
  @Input() oldSlotId!: number;
  @Input() action!: string;
  @Input() appointmentRequestId!: number;




  next7Days: { label: string; date: Date }[] = [];
  selectedDate?: Date;
  allSlots: any[] = [];
  filteredSlots: any[] = [];
  isLoading = false;
  errorMsg = '';
  selectedSlotId?: number;

  constructor(
    public activeModal: NgbActiveModal,
    private doctorService: DoctorsService,
    private appointmentRequestService : AppointmentRequestService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.generateNext14Days();
    this.fetchDoctorSlots();
  }

  generateNext14Days() {
    const today = new Date();
    this.next7Days = [];
    for (let i = 0; i < 14; i++) {
      const date = new Date(today);
      date.setDate(today.getDate() + i);
      const label = date.toLocaleDateString('en-US', {
        weekday: 'short',
        day: 'numeric',
        month: 'short'
      });
      this.next7Days.push({ label, date });
    }
  }

  fetchDoctorSlots() {
    this.isLoading = true;
    this.errorMsg = '';

    this.doctorService.GetDoctorAvailableSlots(this.doctorId).subscribe({
      next: (response) => {
        this.isLoading = false;

        console.log('✅ API Response received:', response);

        if (Array.isArray(response)) {
          this.allSlots = response;
        } else {
          this.errorMsg = response?.Message || 'No slots found.';
        }
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMsg = 'Error fetching slots.';
        console.error(err);
      }
    });
  }

  selectDate(date: Date) {
    this.selectedDate = date;
  
    const selectedDateOnly = date.toDateString(); // locale safe comparison
  
    this.filteredSlots = this.allSlots.filter(s => {
      const slotDate = new Date(s.SlotDate).toDateString();
      return slotDate === selectedDateOnly && s.StatusName === 'Available';
    });
  
    console.log('📅 Selected Date:', selectedDateOnly);
    console.log('🎯 Filtered Slots:', this.filteredSlots);
  
    this.selectedSlotId = undefined;
  }

  formatTime(timeStr: string): string {
    if (!timeStr) return '-';
    const [hour, minute] = timeStr.split(':').map(Number);
    const ampm = hour >= 12 ? 'PM' : 'AM';
    const formattedHour = hour % 12 || 12;
    return `${formattedHour}:${minute.toString().padStart(2, '0')} ${ampm}`;
  }
  

  selectSlot(slotId: number) {
    this.selectedSlotId = slotId;
  }

  // confirmAppointment() {
  //   const selectedSlot = this.filteredSlots.find(s => s.SlotId === this.selectedSlotId);
  //   if (!selectedSlot) {
  //     alert('Please select a slot before confirming.');
  //     return;
  //   }
  
  //   this.activeModal.close();
  //   debugger;
  //   this.router.navigate(['patient/patientdetails'], {
  //     queryParams: {
  //       doctorId: this.doctorId,
  //       doctorName: this.doctorName,
  //       doctorEmail : this.doctorEmail,
  //       slotId: selectedSlot.SlotId,
  //       startTime: selectedSlot.StartTime,
  //       endTime: selectedSlot.EndTime,
  //       slotDate: selectedSlot.SlotDate,
  //       specializationId : this.SpecializationId
  //     }
  //   });
  // }

  confirmAppointment() {
  const selectedSlot = this.filteredSlots.find(s => s.SlotId === this.selectedSlotId);
  if (!selectedSlot) {
    alert('Please select a slot before confirming.');
    return;
  }
  debugger;
  const appointmentModel: any = {
    DoctorId: this.doctorId,
    SpecializationId: this.SpecializationId,
    OldSlotId : this.oldSlotId,
    SlotId: selectedSlot.SlotId,
    PreferredDate: selectedSlot.SlotDate,
    StartTime: selectedSlot.StartTime,
    EndTime: selectedSlot.EndTime,
    DoctorName: this.doctorName,
    DoctorEmail: this.doctorEmail,
    PatientName: this.patientName,
    Action : this.action,
    PatientEmail: this.patientEmail
  };

  if (this.appointmentRequestId) {

    appointmentModel.AppointmentRequestId = this.appointmentRequestId;

    this.appointmentRequestService.RescheduleAppointment(appointmentModel).subscribe({
      next: (response) => {
        alert('Appointment rescheduled successfully.');        
        this.activeModal.close('rescheduled');
      },
      error: (err) => {
        console.error(err);
        alert('Failed to reschedule appointment.');
      }
    });
  } else {
    // 👤 Patient booking new appointment
    this.activeModal.close();
    this.router.navigate(['patient/patient-form'], {
      queryParams: {
        doctorId: appointmentModel.DoctorId,
        doctorName: appointmentModel.DoctorName,
        doctorEmail: appointmentModel.DoctorEmail,
        slotId: appointmentModel.SlotId,
        startTime: appointmentModel.StartTime,
        endTime: appointmentModel.EndTime,
        slotDate: appointmentModel.PreferredDate,
        specializationId: appointmentModel.SpecializationId
      }
    });
  }
}


  
  
}
