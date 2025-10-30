import { Component } from '@angular/core';
import { DoctorsModel } from '../../../core/models/DoctorsModel';
import { DoctorsService } from '../../../core/services/doctors-service';
import { Router } from '@angular/router';
import { ToastService } from '../../../core/services/toast-service';

@Component({
  selector: 'app-doctor-list',
  standalone: false,
  templateUrl: './doctor-list.html',
  styleUrl: './doctor-list.scss'
})
export class DoctorList {

  doctorsList: DoctorsModel[] = [];
  filteredDoctors: DoctorsModel[] = [];
  doctorDetail: DoctorsModel | null = null;
  specializationsList: any[] = [];
  loading = false;
  searchTerm: string = '';
  TotalRecords: number = 0;
  selectedSpecialization: string;
  filters: DoctorsModel = new DoctorsModel();

  constructor(
    private doctorsService: DoctorsService,
     private router: Router,
    private toastService : ToastService) { }

  ngOnInit(): void {
    const savedFilters = sessionStorage.getItem('DoctorListFilters');
    if (savedFilters) {
      this.filters = JSON.parse(savedFilters);
    } else {
      this.filters = new DoctorsModel();
    }
    this.loadDoctors();
  }

  loadDoctors() {
    this.loading = true;
    this.doctorsService.DoctorsGetList(this.filters).subscribe({
      next: (data: any) => {
        this.doctorsList = data.DoctorsList || [];
        this.specializationsList = data.SpecializationsList || [];
        this.filteredDoctors = [...this.doctorsList];
        this.TotalRecords = data.TotalRecords ;
        this.doctorDetail = this.doctorsList.length > 0 ? this.doctorsList[0] : null;
        this.loading = false;
      },
      error: (err) => {
        if ((err as any).isAuthError) return; 
        this.toastService.show("Error loading doctors", { classname: 'bg-danger text-white', delay: 1500 })
        this.loading = false;
      }
    });
  }

  onFilterChange() {
    this.filters.PageNumber = 1;
    sessionStorage.setItem('DoctorListFilters', JSON.stringify(this.filters));
    this.loadDoctors();
  }

  openAddEditDoctor(DoctorId: number = 0) {
    sessionStorage.setItem('DoctorListFilters', JSON.stringify(this.filters));
    if (DoctorId > 0) {
      this.router.navigate(['admin/edit-doctor', DoctorId]);
    } else {
      this.router.navigate(['admin/add-doctor']);
    }
  }

  ClearFilters() {
    this.filters = new DoctorsModel();
    sessionStorage.removeItem('DoctorListFilters');
    this.loadDoctors();
  }
  openDoctorAvailability(doctorId: number) {
    sessionStorage.setItem('DoctorListFilters', JSON.stringify(this.filters));
    this.router.navigate(['admin/edit-availability', doctorId]);

  }




}
