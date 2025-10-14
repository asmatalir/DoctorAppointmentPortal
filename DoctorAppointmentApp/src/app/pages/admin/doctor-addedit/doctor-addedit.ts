import { Component } from '@angular/core';
import { DoctorsService } from '../../../core/services/doctors-service';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { DoctorsModel } from '../../../core/models/DoctorsModel';
import { Router,ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';


@Component({
  selector: 'app-doctor-addedit',
  standalone: false,
  templateUrl: './doctor-addedit.html',
  styleUrl: './doctor-addedit.scss'
})
export class DoctorAddedit {
  
doctor : DoctorsModel = new DoctorsModel();
SpecializationIds : number[]=[];
QualificationIds : number[] = []; 
specializationsList: any[] = [];
qualificationsList: any[] = [];
statesList: any[] = [];
districtsList: any[] = [];
talukasList: any[] = [];
citiesList: any[] = [];


constructor(
  private doctorService: DoctorsService,
  private route: ActivatedRoute,
  private router: Router,
) {}

ngOnInit(): void {
  const doctorId = this.route.snapshot.paramMap.get('id');
  this.loadDoctorDetails(doctorId ? +doctorId : 0);
}

  dropdownSettings: IDropdownSettings = {
    singleSelection: false,
    idField: 'PublisherId',
    textField: 'PublisherName',
    selectAllText: 'Select All',
    unSelectAllText: 'Unselect All',
    itemsShowLimit: 2,
    allowSearchFilter: true
  };

  loadDoctorDetails(id: number = 0) {
    this.doctorService.GetDoctorDetails(id).subscribe({
      next: (response: DoctorsModel) => {
        if (id > 0) {
          // Edit mode
          this.doctor = response;
        } else {
          // Add mode - initialize new model
          this.doctor = new DoctorsModel();
        }

        // Populate dropdown lists
        this.specializationsList = response.SpecializationsList || [];
        this.qualificationsList = response.QualificationsList || [];
        this.statesList = response.StatesList || [];
        this.districtsList = response.DistrictsList || [];
        this.talukasList = response.TalukasList || [];
        this.citiesList = response.CitiesList || [];

        if (this.doctor.DateOfBirth) {
          const dob = new Date(this.doctor.DateOfBirth);
          this.doctor.DateOfBirth = dob.toISOString().substring(0, 10); 
        } else {
          this.doctor.DateOfBirth = ''; // empty if null
        }
        this.SpecializationIds = this.doctor.SpecializationIds? this.doctor.SpecializationIds.split(',').map(id => +id.trim()) : [];
      
        this.QualificationIds = this.doctor.QualificationIds ? this.doctor.QualificationIds.split(',').map(id => +id.trim())  : [];
      

      },
      error: (err) => {
        console.error('Error loading doctor details:', err);
      }
    });
  }
  onSubmit(form: NgForm) {
    if (form.invalid) {
      // this.toast.warning('Please fill all required fields!');
      console.error('Please fill all required fields!');

      return;
    }
    this.doctor.SpecializationIds = this.SpecializationIds.join(',');
    this.doctor.QualificationIds = this.QualificationIds.join(',');

    this.doctorService.SaveDoctorDetails(this.doctor).subscribe({
      next: (response: any) => {
        if (response.success) {
          debugger;
          // this.toast.success('Doctor details saved successfully!');
          console.error('Doctor details saved successfully!');

          this.router.navigate(['/doctors']);  // navigate to doctor list page
          form.form.markAsPristine();
        } else {
          // this.toast.error('Error saving doctor details!');
        }
      },
      error: (err) => {
        debugger;
        console.error('Error saving doctor details', err);
        // this.toast.error('Error saving doctor details!');
      }
    });
  }
  
}
