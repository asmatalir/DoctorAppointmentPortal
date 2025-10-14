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
  specializationsList : SpecializationsModel[] = [];
  loading: boolean = false;
  filters : DoctorsModel = new DoctorsModel();

  constructor(private doctorsService: DoctorsService) { }

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


        this.doctorsList = this.doctorsList.map(doc => {
          return {
            ...doc,
            Specializations: doc.SpecializationNames
              ? doc.SpecializationNames.split(',').map(s => s.trim()).filter(s => s)
              : []  // empty array if no specializations
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
  


}
