import { Component } from '@angular/core';
import { DoctorsModel } from '../../../core/models/DoctorsModel';
import { DoctorsService } from '../../../core/services/doctors-service';
import { Router } from '@angular/router';

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
  TotalRecords : number=0;
  selectedSpecialization : string;
  filters : DoctorsModel = new DoctorsModel();

  constructor(private doctorsService: DoctorsService,private router: Router) { }

  ngOnInit(): void {
    this.loadDoctors();
  }

  loadDoctors() {
    this.loading = true;
    this.doctorsService.DoctorsGetList(this.filters).subscribe({
      next: (data: any) => {
        this.doctorsList = data.DoctorsList || [];
        this.specializationsList = data.SpecializationsList || [];
        this.filteredDoctors = [...this.doctorsList];
        this.TotalRecords = data.TotalRecords || this.doctorsList.length;
        this.doctorDetail = this.doctorsList.length > 0 ? this.doctorsList[0] : null;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading doctors:', err);
        this.loading = false;
      }
    });
  }

  onFilterChange() {
    this.filters.PageNumber = 1; 
    this.loadDoctors();
  }

  openAddEditDoctor(DoctorId : number = 0){
    if (DoctorId > 0) {
      this.router.navigate(['admin/edit-doctor', DoctorId]);
    } else {
      this.router.navigate(['admin/add-doctor']);
    }
  }
  
  ClearFilters(){
    this.filters = new DoctorsModel();
    this.loadDoctors();
   }
openDoctorAvailability(doctorId: number) {
  this.router.navigate(['admin/edit-availability', doctorId]);

}


  

}
