import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { DoctorsModel } from '../../../core/models/DoctorsModel';
import { DoctorsService } from '../../../core/services/doctors-service';
import { SpecializationsModel } from '../../../core/models/SpecializationsModel';
import { QualificationsModel } from '../../../core/models/QualificationsModel';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DoctorAvailableSlotsModal } from '../doctor-available-slots-modal/doctor-available-slots-modal';
import { CitiesModel } from '../../../core/models/CitiesModel';
import { ToastService } from '../../../core/services/toast-service';


@Component({
  selector: 'app-doctors-list',
  standalone: false,
  templateUrl: './doctors-list.html',
  styleUrl: './doctors-list.scss'
})

export class DoctorsList implements OnInit {

  doctorsList: DoctorsModel[] = [];
  specializationsList: SpecializationsModel[] = [];
  qualificationsList: QualificationsModel[] = [];
  CitiesList: CitiesModel[] = [];
  filters: DoctorsModel = new DoctorsModel();
  TotalRecords: number = 0;


  constructor(
    private doctorsService: DoctorsService,
    private modalService: NgbModal,
    private toastService: ToastService) { }

  ngOnInit(): void {
    const savedFilters = sessionStorage.getItem('DoctorFilters');
    if (savedFilters) {
      this.filters = JSON.parse(savedFilters);
    } else {
      this.filters = new DoctorsModel();
    }
    this.loadDoctors();
  }

  getProfileImage(doctor: any): string {
    return doctor.Gender === 'M' ? '/images/defaultprofilepicmale.jpg' : '/images/defaultprofilepicfemale.jpg';
  }


  loadDoctors() {
    sessionStorage.setItem("DoctorFilters", JSON.stringify(this.filters));
    this.doctorsService.DoctorsGetList(this.filters).subscribe({
      next: (data: any) => {
        this.doctorsList = data.DoctorsList || [];
        this.specializationsList = data.SpecializationsList || [];
        this.qualificationsList = data.QualificationsList || [];
        this.CitiesList = data.CitiesList || [];
        this.TotalRecords = data.TotalRecords;

        this.doctorsList = this.doctorsList.map(doc => {
          return {
            ...doc, Specializations: doc.SpecializationNames ? doc.SpecializationNames.split(',').map(s => s.trim()).filter(s => s) : [], Qualifications: doc.QualificationNames ? doc.QualificationNames.split(',').map(s => s.trim()).filter(s => s) : []
          };
        });
      },
      error: (err) => {
        if ((err as any).isAuthError) return;
        this.toastService.show("Error loading doctors", { classname: 'bg-danger text-white', delay: 1500 });
      }
    });
  }


  openBookingModal(doctor: any) {
    const modalRef = this.modalService.open(DoctorAvailableSlotsModal, { size: 'lg', centered: true });

    //Setting the modal paramaters values
    modalRef.componentInstance.doctorId = doctor.DoctorId;
    modalRef.componentInstance.doctorName = `${doctor.FirstName} ${doctor.LastName}`;
    modalRef.componentInstance.doctorEmail = doctor.DoctorEmail;
    modalRef.componentInstance.SpecializationId = this.filters.SelectedSpecializationId;


    debugger;
    modalRef.result.then(
      (result) => {
        if (result) {
          this.toastService.show("Appointment confirmed.Please check your mail.", { classname: 'bg-danger text-white', delay: 1500 });
        }
      },
      () => { }
    );
  }

  clearFilters() {
    this.filters.SelectedSpecializationId = null;
    this.filters.SelectedCityId = null;
    this.filters.Gender = '';
    sessionStorage.removeItem("DoctorFilters");
    this.loadDoctors();
  }


}
