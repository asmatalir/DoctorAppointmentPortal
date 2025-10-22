import { Component,ViewChild,TemplateRef } from '@angular/core';
import { AppointmentRequestsModel } from '../../../core/models/AppointmentRequestsModel';
import { SpecializationsModel } from '../../../core/models/SpecializationsModel';
import { AppointmentRequestService } from '../../../core/services/appointment-request-service';
import { NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap';
import { StatusesModel } from '../../../core/models/StatusesModel';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DoctorAvailableSlotsModal } from '../../Patient/doctor-available-slots-modal/doctor-available-slots-modal';

@Component({
  selector: 'app-doctor-appointment-requests',
  standalone: false,
  templateUrl: './doctor-appointment-requests.html',
  styleUrl: './doctor-appointment-requests.scss'
})
export class DoctorAppointmentRequests {
  loading : boolean = false;
  filters : AppointmentRequestsModel = new AppointmentRequestsModel();
  model : AppointmentRequestsModel = new AppointmentRequestsModel();
  AppointmentRequestList : AppointmentRequestsModel[]=[];
  specializationsList : SpecializationsModel[] = [];
  statusesList : StatusesModel[] = [];
  TotalRecords : number = 0;
  
  @ViewChild('filterOffcanvas') offcanvasRef: any;

  constructor(private appointmentRequestService : AppointmentRequestService,
    private offcanvas: NgbOffcanvas,
    private modalService: NgbModal
  ) { }



  ngOnInit(): void {
    this.loadAppointmentRequests();

    
  }

  loadAppointmentRequests() {
    this.loading = true;
    this.appointmentRequestService.DoctorAppointmentRequestGetList(this.filters).subscribe({
      next: (data: any) => {
        // Extract the actual doctors array from the response
        this.AppointmentRequestList = data.AppointmentRequestList || [];
        this.specializationsList = data.SpecializationsList || [];
        this.statusesList = data.StatusesList || [];
        this.TotalRecords = data.TotalRecords;       
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading appointment requests:', err);
        this.loading = false;
      }
    });
  }

   openBookingModal(doctor: any,action : string) {
      const modalRef = this.modalService.open(DoctorAvailableSlotsModal, { size: 'lg', centered: true });
    
      // ✅ Set inputs correctly
      doctor.SelectedSpecializationId=1;
      modalRef.componentInstance.doctorId = doctor.DoctorId;
      modalRef.componentInstance.doctorName = doctor.DoctorName;
      modalRef.componentInstance.doctorEmail = doctor.DoctorEmail;
      modalRef.componentInstance.patientName = doctor.PatientName;
      modalRef.componentInstance.patientEmail = doctor.PatientEmail;
      modalRef.componentInstance.appointmentRequestId = doctor.AppointmentRequestId;
      modalRef.componentInstance.oldSlotId = doctor.SlotId;
      modalRef.componentInstance.action = action;
      modalRef.componentInstance.SpecializationId = doctor.SelectedSpecializationId;
  
       
      debugger;
      modalRef.result.then(
        (result) => {
          if (result === 'rescheduled') {  
            console.log('Appointment rescheduled, reloading list...');
            this.loadAppointmentRequests(); 
          }
          else {
            console.log('Modal closed with result:', result);
          }
        },
        () => {} 
      );
    }

  openEnd(content: TemplateRef<any>) {
    this.offcanvas.open(content, { position: 'end' });
  }

  applyFilters(offcanvas: any) {
    this.filters.PageNumber = 1;
    this.loadAppointmentRequests();
    offcanvas.dismiss();
  }

  clearFilters(offcanvas: any) {
    this.filters=new AppointmentRequestsModel();
    this.loadAppointmentRequests();
    offcanvas.dismiss();
  }
  updateAppointmentStatus(appointment: any, status: string) {
    if(status == 'Accepted')
    {
      this.model.Action = 'Approved';
    }
    else
    {
      this.model.Action = 'Rejected';
    }
    this.model.PatientEmail = appointment.PatientEmail;
    this.model.AppointmentRequestId= appointment.AppointmentRequestId;
    this.model.FinalDate = appointment.FinalDate;
    this.model.DoctorName = appointment.DoctorName;
    this.model.PatientName = appointment.PatientName;
    this.model.FinalStartTime = appointment.FinalStartTime;
    this.model.FinalEndTime = appointment.FinalEndTime;
    // this.model = appointment;
    console.log("Appointment Id  " + this.model.AppointmentRequestId)
    console.log("Patient Email " + this.model.PatientEmail)
    this.appointmentRequestService.UpdateStatus(this.model)
        .subscribe({
            next: () => {
                alert(`Appointment ${status.toLowerCase()} and emails sent!`);
                this.loadAppointmentRequests(); 
            },
            error: (err) => console.error(err)
        });
  }
  
  rescheduleAppointment(){}
  onFilterChange(){
    this.loadAppointmentRequests();
  }
}
