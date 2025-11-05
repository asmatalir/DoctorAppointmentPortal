import { Component, ViewChild, TemplateRef } from '@angular/core';
import { AppointmentRequestsModel } from '../../../core/models/AppointmentRequestsModel';
import { SpecializationsModel } from '../../../core/models/SpecializationsModel';
import { AppointmentRequestService } from '../../../core/services/appointment-request-service';
import { NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap';
import { StatusesModel } from '../../../core/models/StatusesModel';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DoctorAvailableSlotsModal } from '../../Patient/doctor-available-slots-modal/doctor-available-slots-modal';
import { ToastService } from '../../../core/services/toast-service';

@Component({
  selector: 'app-doctor-appointment-requests',
  standalone: false,
  templateUrl: './doctor-appointment-requests.html',
  styleUrl: './doctor-appointment-requests.scss'
})
export class DoctorAppointmentRequests {
  loading: boolean = false;
  filters: AppointmentRequestsModel = new AppointmentRequestsModel();
  model: AppointmentRequestsModel = new AppointmentRequestsModel();
  AppointmentRequestList: AppointmentRequestsModel[] = [];
  specializationsList: SpecializationsModel[] = [];
  statusesList: StatusesModel[] = [];
  TotalRecords: number = 0;

  @ViewChild('filterOffcanvas') offcanvasRef: any;

  constructor(private appointmentRequestService: AppointmentRequestService,
    private offcanvas: NgbOffcanvas,
    private modalService: NgbModal,
    private toastService: ToastService
  ) { }



  ngOnInit(): void {
    this.loadAppointmentRequests();


  }

  loadAppointmentRequests() {
    this.loading = true;

    if (this.filters.FromDate && this.filters.ToDate) {
      if (new Date(this.filters.ToDate) < new Date(this.filters.FromDate)) {
        this.toastService.show("To Date cannot be earlier than From Date", { classname: 'bg-warning text-white', delay: 1500 });
        this.filters.FromDate='';
        this.filters.ToDate='';
        this.loadAppointmentRequests();
        return;
      }
    }

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
        if ((err as any).isAuthError) return;
        this.toastService.show(`Error: ${err?.error?.message || err?.error || err?.message || "An unexpected error occurred."}`, { classname: 'bg-danger text-white', delay: 1500 });
        this.loading = false;
      }
    });
  }

  openBookingModal(appointment: any, action: string) {
    debugger;
    const modalRef = this.modalService.open(DoctorAvailableSlotsModal, { size: 'lg', centered: true });
    debugger;
    // ✅ Set inputs correctly
    appointment.SelectedSpecializationId = 1;
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
          this.toastService.show("Appointment rescheduled", { classname: 'bg-success text-white', delay: 1500 })
          this.loadAppointmentRequests();
        }
        else {
          this.toastService.show(`${{ result }}`, { classname: 'bg-success text-white', delay: 1500 })

        }
      },
      () => { }
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
    this.filters = new AppointmentRequestsModel();
    this.loadAppointmentRequests();
    offcanvas.dismiss();
  }
  updateAppointmentStatus(appointment: any, status: string) {
    if (status == 'Accepted') {
      appointment.Action = 'Approved';
    }
    else {
      appointment.Action = 'Rejected';
    }
    appointment.StartTime = appointment.FinalStartTime;
    appointment.EndTime = appointment.FinalEndTime;
    appointment.PreferredDate = appointment.FinalDate;

    this.appointmentRequestService.UpdateStatus(appointment)
      .subscribe({
        next: () => {
          this.toastService.show(`Appointment ${status.toLowerCase()} successfully`, { classname: 'bg-success text-white', delay: 1500 });
          this.loadAppointmentRequests();
        },
        error: (err) => {
          if ((err as any).isAuthError) return;
        this.toastService.show(`Error: ${err?.error?.message || err?.error || err?.message || "An unexpected error occurred."}`, { classname: 'bg-danger text-white', delay: 1500 });
        }

      });
  }

  onFilterChange() {
    this.loadAppointmentRequests();
  }
}
