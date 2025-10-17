import { Component, OnInit,ViewChild,TemplateRef } from '@angular/core';
import { DoctorsModel } from '../../../core/models/DoctorsModel';
import { DoctorsService } from '../../../core/services/doctors-service';
import { SpecializationsModel } from '../../../core/models/SpecializationsModel';
import { QualificationsModel } from '../../../core/models/QualificationsModel';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { DoctorAvailabileSlotsModel } from '../../../core/models/DoctorAvailableSlotsModel';
import { DoctorAvailableSlotsModal } from '../doctor-available-slots-modal/doctor-available-slots-modal';


@Component({
  selector: 'app-doctors-list',
  standalone: false,
  templateUrl: './doctors-list.html',
  styleUrl: './doctors-list.scss'
})
export class DoctorsList implements OnInit {
  doctorsList: DoctorsModel[] = [];
  specializationsList : SpecializationsModel[] = [];
  qualificationsList : QualificationsModel[] = [];
  loading: boolean = false;
  filters : DoctorsModel = new DoctorsModel();
  TotalRecords : number = 0;


  constructor(private doctorsService: DoctorsService , private modalService: NgbModal) {
     
    }

  ngOnInit(): void {
    this.loadDoctors();

    
  }

  loadDoctors() {
    this.loading = true;
    this.doctorsService.DoctorsGetList(this.filters).subscribe({
      next: (data: any) => {
        debugger;
        // Extract the actual doctors array from the response
        this.doctorsList = data.DoctorsList || [];
        this.specializationsList = data.SpecializationsList || [];
        this.qualificationsList = data.QualificationsList || [];
        this.TotalRecords = data.TotalRecords;

        this.doctorsList = this.doctorsList.map(doc => {

          return {
            ...doc,
            Specializations: doc.SpecializationNames
              ? doc.SpecializationNames.split(',').map(s => s.trim()).filter(s => s)
              : [],
            Qualifications: doc.QualificationNames
              ? doc.QualificationNames.split(',').map(s => s.trim()).filter(s => s)
              : []
          };
        });
        
                

        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading doctors:', err);
        this.loading = false;
      }
    });
  }

  
  openBookingModal(doctor: any) {
    const modalRef = this.modalService.open(DoctorAvailableSlotsModal, { size: 'lg', centered: true });
  
    // ✅ Set inputs correctly
    doctor.SelectedSpecializationId=1;
    modalRef.componentInstance.doctorId = doctor.DoctorId;
    modalRef.componentInstance.doctorName = `${doctor.FirstName} ${doctor.LastName}`;
    modalRef.componentInstance.SpecializationId = doctor.SelectedSpecializationId;
     
    debugger;
    modalRef.result.then(
      (result) => {
        if (result) {
          console.log('Appointment confirmed:', result);
          // Optional: Call an API to save appointment
        }
      },
      () => {} // modal dismissed
    );
  }
  
  groupSlotsByDay(slots: any[]) {
    const grouped: any = {};
    slots.forEach((slot: any) => {
      const dateKey = new Date(slot.SlotDate).toISOString().split('T')[0];
      if (!grouped[dateKey]) grouped[dateKey] = { Morning: [], Afternoon: [], Evening: [] };
  
      const hour = parseInt(slot.StartTime.split(':')[0], 10);
      const isPM = slot.StartTime.includes('PM');
      let period: 'Morning' | 'Afternoon' | 'Evening';
      if (!isPM && hour < 12) period = 'Morning';
      else if (isPM && hour < 5) period = 'Afternoon';
      else period = 'Evening';
  
      grouped[dateKey][period].push(slot);
    });
    return grouped;
  }
  

}
