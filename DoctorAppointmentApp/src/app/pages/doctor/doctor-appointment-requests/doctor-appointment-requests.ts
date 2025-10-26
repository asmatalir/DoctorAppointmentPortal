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

   openBookingModal(appointment: any,action : string) {
    debugger;
      const modalRef = this.modalService.open(DoctorAvailableSlotsModal, { size: 'lg', centered: true });
      debugger;
      // ✅ Set inputs correctly
      appointment.SelectedSpecializationId=1;
      modalRef.componentInstance.doctorId = appointment.DoctorId;
      modalRef.componentInstance.doctorName = appointment.DoctorName;
      modalRef.componentInstance.doctorEmail = appointment.DoctorEmail;
      modalRef.componentInstance.patientName = appointment.PatientName;
      modalRef.componentInstance.patientEmail = appointment.PatientEmail;
      modalRef.componentInstance.appointmentRequestId = appointment.AppointmentRequestId;
      modalRef.componentInstance.oldSlotId = appointment.SlotId;
      modalRef.componentInstance.action = action;
      modalRef.componentInstance.SpecializationId = appointment.SelectedSpecializationId;
  
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
      appointment.Action = 'Approved';
    }
    else
    {
      appointment.Action = 'Rejected';
    }
    // this.model.PatientEmail = appointment.PatientEmail;
    // this.model.AppointmentRequestId= appointment.AppointmentRequestId;
    // this.model.FinalDate = appointment.FinalDate;
    // this.model.DoctorName = appointment.DoctorName;
    // this.model.DoctorEmail = appointment.DoctorEmail;
    // this.model.PatientName = appointment.PatientName;
    // this.model.FinalStartTime = appointment.FinalStartTime;
    // this.model.FinalEndTime = appointment.FinalEndTime;
    // this.model = appointment;
    appointment.StartTime = appointment.FinalStartTime;
    appointment.EndTime = appointment.FinalEndTime;
    appointment.PreferredDate = appointment.FinalDate;
    debugger;
    console.log("Appointment Id  " + appointment.startTime)
    console.log("Patient Email " + appointment.startTime)
    this.appointmentRequestService.UpdateStatus(appointment)
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
