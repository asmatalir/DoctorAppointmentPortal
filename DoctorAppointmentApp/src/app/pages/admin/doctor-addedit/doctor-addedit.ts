import { Component } from '@angular/core';
import { DoctorsService } from '../../../core/services/doctors-service';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { DoctorsModel } from '../../../core/models/DoctorsModel';
import { Router,ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';
import { LocationService } from '../../../core/services/location-service';
import { ToastService } from '../../../core/services/toast-service';


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
today: string;


constructor(
  private doctorService: DoctorsService,
  private route: ActivatedRoute,
  private router: Router,
  private locationService : LocationService,
  private toastService : ToastService
) {}

ngOnInit(): void {
  const now = new Date();
  this.today = now.toISOString().split('T')[0];
  const doctorId = this.route.snapshot.paramMap.get('id');
  this.loadDoctorDetails(doctorId ? +doctorId : 0);
  this.loadStates();
}

    // Load all states
  loadStates() {
    this.locationService.getStates().subscribe({
      next: data => this.statesList = data,
      error: err => console.error('Error loading states', err)
    });
  }

  // Load districts (optionally select a district in edit mode)
  loadDistricts(stateId: number, selectedDistrictId?: number) {
    this.locationService.getDistricts(stateId).subscribe({
      next: districts => {
        this.districtsList = districts;

        if (selectedDistrictId) {
          this.doctor.DistrictId = selectedDistrictId;
          this.loadTalukas(this.doctor.DistrictId, this.doctor.TalukaId as number);
        }
      },
      error: err => console.error('Error loading districts', err)
    });
  }

  // Load talukas (optionally select a taluka in edit mode)
  loadTalukas(districtId: number, selectedTalukaId?: number) {
    this.locationService.getTalukas(districtId).subscribe({
      next: talukas => {
        this.talukasList = talukas;

        if (selectedTalukaId) {
          this.doctor.TalukaId = selectedTalukaId;
          this.loadCities(this.doctor.TalukaId, this.doctor.CityId as number);
        }
      },
      error: err => console.error('Error loading talukas', err)
    });
  }

  // Load cities (optionally select a city in edit mode)
  loadCities(talukaId: number, selectedCityId?: number) {
    this.locationService.getCities(talukaId).subscribe({
      next: cities => {
        this.citiesList = cities;

        if (selectedCityId) {
          this.doctor.CityId = selectedCityId;
        }
      },
      error: err => console.error('Error loading cities', err)
    });
  }

  // User changes state
  onStateChange() {

    this.doctor.DistrictId = 0;
    this.doctor.TalukaId = 0;
    this.doctor.CityId = 0;
    this.districtsList = [];
    this.talukasList = [];
    this.citiesList = [];

    if (this.doctor.StateId) {
      this.loadDistricts(this.doctor.StateId);
    }
  }

  // User changes district
  onDistrictChange() {

     if (!this.doctor.StateId) {
    // this.toastr.warning('Please select a state first.');
    this.toastService.show("Please select a state first.", { classname: 'bg-warning text-white', delay: 1500 });
    this.doctor.DistrictId = 0; // reset selection
    return;
  }
    this.doctor.TalukaId = 0;
    this.doctor.CityId = 0;
    this.talukasList = [];
    this.citiesList = [];

    if (this.doctor.DistrictId) {
      this.loadTalukas(this.doctor.DistrictId);
    }
  }

  onTalukaChange() {

    if (!this.doctor.TalukaId) {
    this.toastService.show("Please select a District first.", { classname: 'bg-warning text-white', delay: 1500 });
    this.doctor.TalukaId = 0; 
    return;
  }
    this.doctor.CityId = 0;
    this.citiesList = [];

    if (this.doctor.TalukaId) {
      this.loadCities(this.doctor.TalukaId);
    }
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
          if (this.doctor.StateId) {
          this.loadDistricts(this.doctor.StateId, this.doctor.DistrictId as number);
        }
        } else {
          // Add mode - initialize new model
          this.doctor = new DoctorsModel();

        }

        // Populate dropdown lists
        this.specializationsList = response.SpecializationsList || [];
        this.qualificationsList = response.QualificationsList || [];

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

  calculateAge(dob: string): number {
  const birth = new Date(dob);
  const today = new Date();
  let age = today.getFullYear() - birth.getFullYear();
  const m = today.getMonth() - birth.getMonth();
  if (m < 0 || (m === 0 && today.getDate() < birth.getDate())) {
    age--;
  }
  return age;
}

  
  onSubmit(form: NgForm) {
      if (form.invalid) {
        alert('Please fill all required fields before submitting.');
    form.control.markAllAsTouched();
    return;
  }

  // Additional explicit checks
  const age = this.calculateAge(this.doctor.DateOfBirth);
  if (age < 25 || age > 100) {
    alert("Doctor's age should be between 25 and 125 years");
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

          this.router.navigate(['admin/doctor-list']);  // navigate to doctor list page
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

  onCancel() {
  this.router.navigate(['admin/doctor-list']); 
}

  
}
