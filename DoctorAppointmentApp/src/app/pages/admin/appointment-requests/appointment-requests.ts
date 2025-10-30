import { Component, ViewChild, TemplateRef } from '@angular/core';
import { AppointmentRequestsModel } from '../../../core/models/AppointmentRequestsModel';
import { AppointmentRequestService } from '../../../core/services/appointment-request-service';
import { SpecializationsModel } from '../../../core/models/SpecializationsModel';
import { NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap';
import { StatusesModel } from '../../../core/models/StatusesModel';
import { ToastService } from '../../../core/services/toast-service';


@Component({
  selector: 'app-appointment-requests',
  standalone: false,
  templateUrl: './appointment-requests.html',
  styleUrl: './appointment-requests.scss'
})
export class AppointmentRequests {
  loading: boolean = false;
  filters: AppointmentRequestsModel = new AppointmentRequestsModel();
  AppointmentRequestList: AppointmentRequestsModel[] = [];
  specializationsList: SpecializationsModel[] = [];
  statusesList: StatusesModel[] = [];
  TotalRecords: number = 0;

  @ViewChild('filterOffcanvas') offcanvasRef: any;

  constructor(private appointmentRequestService: AppointmentRequestService,
    private offcanvas: NgbOffcanvas,
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
    this.appointmentRequestService.AppointmentRequestGetList(this.filters).subscribe({
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
        this.toastService.show("Error loading appointment requests", { classname: 'bg-danger text-white', delay: 1500 })
        this.loading = false;
      }
    });
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
  onFilterChange() {
    this.loadAppointmentRequests();
  }
}
