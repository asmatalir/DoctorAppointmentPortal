import { Component, OnInit } from '@angular/core';
import { DoctorsModel } from '../../core/models/DoctorsModel';
import { DoctorsService } from '../../core/services/doctors-service';
import { SpecializationsModel } from '../../core/models/SpecializationsModel';

@Component({
  selector: 'app-doctors-list',
  standalone: false,
  templateUrl: './doctors-list.html',
  styleUrl: './doctors-list.scss'
})
export class DoctorsList implements OnInit {
  doctorsList: DoctorsModel[] = [];
  doctorDetail: DoctorsModel | null = null;
  specializationsList : SpecializationsModel[] = [];
  loading: boolean = false;

  constructor(private doctorsService: DoctorsService) { }

  ngOnInit(): void {
    this.loadDoctors();
  }

  loadDoctors() {
    this.loading = true;
    this.doctorsService.DoctorsGetList().subscribe({
      next: (data: any) => {
        // Extract the actual doctors array from the response
        this.doctorsList = data.DoctorsList || [];
        this.specializationsList = data.specializationsList || [];
        // Set first doctor as default detail
        this.doctorDetail = this.doctorsList.length > 0 ? this.doctorsList[0] : null;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading doctors:', err);
        this.loading = false;
      }
    });
  }
  

  viewMore(index: number) {
    this.doctorDetail = this.doctorsList[index];
  }
}
