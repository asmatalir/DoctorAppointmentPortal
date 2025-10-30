import { Component,ViewChild,TemplateRef } from '@angular/core';
import { DoctorsService } from '../../../core/services/doctors-service';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { DoctorsModel } from '../../../core/models/DoctorsModel';
import { Router, ActivatedRoute } from '@angular/router';
import { NgForm } from '@angular/forms';
import { LocationService } from '../../../core/services/location-service';
import { ToastService } from '../../../core/services/toast-service';
import { NgbModal,NgbModalRef } from '@ng-bootstrap/ng-bootstrap';


@Component({
  selector: 'app-doctor-addedit',
  standalone: false,
  templateUrl: './doctor-addedit.html',
  styleUrl: './doctor-addedit.scss'
})
export class DoctorAddedit {

  doctor: DoctorsModel = new DoctorsModel();
  SpecializationIds: number[] = [];
  QualificationIds: number[] = [];
  specializationsList: any[] = [];
  qualificationsList: any[] = [];
  statesList: any[] = [];
  districtsList: any[] = [];
  talukasList: any[] = [];
  citiesList: any[] = [];
  today: string;

   @ViewChild('doctorForm') form?: NgForm;
   @ViewChild('unsavedChangesModal') unsavedChangesModal!: TemplateRef<any>; 

  private formSaved = false;
  private modalRef: NgbModalRef;

  constructor(
    private doctorService: DoctorsService,
    private route: ActivatedRoute,
    private router: Router,
    private locationService: LocationService,
    private toastService: ToastService,
    private modalService: NgbModal
  ) { }

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
      error: err => this.toastService.show("Error loading states", { classname: 'bg-danger text-white', delay: 1500 })
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
      error: err => this.toastService.show("Error loading districts", { classname: 'bg-danger text-white', delay: 1500 })
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
      error: err => this.toastService.show("Error loading talukas", { classname: 'bg-danger text-white', delay: 1500 })
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
      error: err => this.toastService.show("Error loading cities", { classname: 'bg-danger text-white', delay: 1500 })
    });
  }

  // User changes state
  onStateChange() {

    this.doctor.DistrictId = null;
    this.doctor.TalukaId = null;
    this.doctor.CityId = null;
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
      this.doctor.DistrictId = null; 
      return;
    }
    this.doctor.TalukaId = null;
    this.doctor.CityId = null;;
    this.talukasList = [];
    this.citiesList = [];

    if (this.doctor.DistrictId) {
      this.loadTalukas(this.doctor.DistrictId);
    }
  }

  onTalukaChange() {

    if (!this.doctor.TalukaId) {
      this.toastService.show("Please select a District first.", { classname: 'bg-warning text-white', delay: 1500 });
      this.doctor.TalukaId = null;
      return;
    }
    this.doctor.CityId = null;
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
        this.SpecializationIds = this.doctor.SpecializationIds ? this.doctor.SpecializationIds.split(',').map(id => +id.trim()) : [];

        this.QualificationIds = this.doctor.QualificationIds ? this.doctor.QualificationIds.split(',').map(id => +id.trim()) : [];


      },
      error: (err) => {
        if ((err as any).isAuthError) return;
        this.toastService.show("Error loading doctor details", { classname: 'bg-danger text-white', delay: 1500 });
      }
    });
  }

    canDeactivate(): Promise<boolean> | boolean {
    if (this.form?.dirty && !this.formSaved) {
      return new Promise((resolve) => {
        this.modalRef = this.modalService.open(this.unsavedChangesModal, { centered: true });
        this.modalRef.result.then(
          (result) => resolve(result === 'discard'),
          () => resolve(false) // closed without confirming
        );
      });
    }
    return true;
  }
    confirmDiscard() {
    this.modalRef?.close('discard');
  }

  stayHere() {
    this.modalRef?.dismiss();
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
      this.toastService.show("Please fill all required fields before submitting", { classname: 'bg-warning text-white', delay: 1500 });
      form.control.markAllAsTouched();
      return;
    }

    // Additional explicit checks
    const age = this.calculateAge(this.doctor.DateOfBirth);
    if (age < 25 || age > 100) {
      this.toastService.show("Doctor's age should be between 25 and 125 years", { classname: 'bg-warning text-white', delay: 1500 });
      return;
    }

    if(this.doctor.ExperienceYears > 100)
    {
        this.toastService.show("Experience cannot exceed 100 years", { classname: 'bg-warning text-white', delay: 1500 });
        return;
    }

    if (this.doctor.ExperienceYears > age - 20) {
      this.toastService.show("Experience seems unrealistic compared to age", { classname: 'bg-warning text-white', delay: 1500 });
      return;
}
    this.doctor.SpecializationIds = this.SpecializationIds.join(',');
    this.doctor.QualificationIds = this.QualificationIds.join(',');

    this.doctorService.SaveDoctorDetails(this.doctor).subscribe({
      next: (response: any) => {
        if (response.success) {
          debugger;
          this.toastService.show("Doctor details saved successfully", { classname: 'bg-success text-white', delay: 1500 });
          this.router.navigate(['admin/doctor-list']); 
          this.formSaved = true;
          form.form.markAsPristine();
        } else {
          this.toastService.show("Error saving doctor details", { classname: 'bg-danger text-white', delay: 1500 });

        }
      },
      error: (err) => {
        if ((err as any).isAuthError) return;
        this.toastService.show("Error saving doctor details", { classname: 'bg-danger text-white', delay: 1500 });        // this.toast.error('Error saving doctor details!');
      }
    });
  }

  onCancel() {
    this.router.navigate(['admin/doctor-list']);
  }


}
