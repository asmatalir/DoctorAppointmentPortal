import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PatientDetailsModel } from '../../../core/models/PatientDetailsModel';
import { NgForm } from '@angular/forms';
import { AppointmentRequestService } from '../../../core/services/appointment-request-service';
import { AppointmentRequestsModel } from '../../../core/models/AppointmentRequestsModel';
import { LocationService } from '../../../core/services/location-service';
import { ToastService } from '../../../core/services/toast-service';


@Component({
  selector: 'app-patient-form',
  standalone: false,
  templateUrl: './patient-form.html',
  styleUrl: './patient-form.scss'
})
export class PatientForm {

  doctorId!: number;
  doctorName: string;
  doctorEmail: string;
  slotId!: number;
  startTime!: string;
  endTime!: string;
  slotDate!: string;
  speializationId: number;
  selectedFile: File | null = null;
  maxFileSizeMB = 5;
  today: string;


  statesList: any[] = [];
  districtsList: any[] = [];
  talukasList: any[] = [];
  citiesList: any[] = [];

  appointment: AppointmentRequestsModel = new AppointmentRequestsModel();

  constructor(private route: ActivatedRoute,
    private appointmentRequestService: AppointmentRequestService,
    private locationService: LocationService,
    private toastService: ToastService,
    private router: Router
  ) { }

  ngOnInit() {
    debugger;

      const now = new Date();
      this.today = now.toISOString().split('T')[0];

    this.route.queryParams.subscribe(params => {
      this.appointment.DoctorId = +params['doctorId'];
      this.appointment.DoctorName = params['doctorName'];
      this.appointment.DoctorEmail = params['doctorEmail'],
      this.appointment.SlotId = +params['slotId'];
      this.appointment.StartTime = params['startTime'];
      this.appointment.EndTime = params['endTime'];
      this.appointment.PreferredDate = params['slotDate'];
      this.appointment.SelectedSpecializationId = params['specializationId'];
    });
    this.loadStates();
  }

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
          this.appointment.DistrictId = selectedDistrictId;
          this.loadTalukas(this.appointment.DistrictId, this.appointment.TalukaId as number);
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
          this.appointment.TalukaId = selectedTalukaId;
          this.loadCities(this.appointment.TalukaId, this.appointment.CityId as number);
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
          this.appointment.CityId = selectedCityId;
        }
      },
      error: err => console.error('Error loading cities', err)
    });
  }

  // User changes state
  onStateChange() {

    this.appointment.DistrictId = 0;
    this.appointment.TalukaId = 0;
    this.appointment.CityId = 0;
    this.districtsList = [];
    this.talukasList = [];
    this.citiesList = [];

    if (this.appointment.StateId) {
      this.loadDistricts(this.appointment.StateId);
    }
  }

  // User changes district
  onDistrictChange() {

    if (!this.appointment.StateId) {
      // this.toastr.warning('Please select a state first.');
      this.toastService.show("Please select a state first.", { classname: 'bg-warning text-white', delay: 1500 });
      this.appointment.DistrictId = 0; // reset selection
      return;
    }
    this.appointment.TalukaId = 0;
    this.appointment.CityId = 0;
    this.talukasList = [];
    this.citiesList = [];

    if (this.appointment.DistrictId) {
      this.loadTalukas(this.appointment.DistrictId);
    }
  }

  onTalukaChange() {

    if (!this.appointment.TalukaId) {
      this.toastService.show("Please select a District first.", { classname: 'bg-warning text-white', delay: 1500 });
      this.appointment.TalukaId = 0;
      return;
    }
    this.appointment.CityId = 0;
    this.citiesList = [];

    if (this.appointment.TalukaId) {
      this.loadCities(this.appointment.TalukaId);
    }
  }


  loadPatientDetails() {
    if (!this.appointment.ContactNumber) {
      return; // Exit if contact number is empty
    }

    const enteredContact = this.appointment.ContactNumber;
    const doctorContext = {
      DoctorId: this.appointment.DoctorId,
      DoctorName: this.appointment.DoctorName,
      DoctorEmail: this.appointment.DoctorEmail,
      SlotId: this.appointment.SlotId,
      StartTime: this.appointment.StartTime,
      EndTime: this.appointment.EndTime,
      PreferredDate: this.appointment.PreferredDate,
      SpecializationId: this.appointment.SelectedSpecializationId
    };

    this.appointmentRequestService.GetPatientDetails(this.appointment.ContactNumber)
      .subscribe({
        next: (response: AppointmentRequestsModel) => {
          // Assign the entire response to the patient model
          Object.assign(this.appointment, response);
          Object.assign(this.appointment, doctorContext);
          this.appointment.ContactNumber = response.ContactNumber || enteredContact;

          if (this.appointment.DateOfBirth) {
            const dob = new Date(this.appointment.DateOfBirth);
            this.appointment.DateOfBirth = dob.toISOString().substring(0, 10);
          } else {
            this.appointment.DateOfBirth = ''; // empty if null
          }
          this.loadDistricts(this.appointment.StateId as number, this.appointment.DistrictId as number);

        },
        error: (err) => {
          console.error("Error loading patient details:", err);
        }
      });
  }

  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      const maxSizeBytes = this.maxFileSizeMB * 1024 * 1024;

      if (file.size > maxSizeBytes) {
        alert(`File size should not exceed ${this.maxFileSizeMB} MB.`);
        event.target.value = '';
        this.selectedFile = null;
        return;
      }

      this.selectedFile = file;
    } else {
      this.selectedFile = null;
    }
  }

  onSubmit(form: NgForm) {
    if (form.invalid) {
      alert('Please fill all required fields before submitting.');
      form.control.markAllAsTouched();
      return;
    }
    const formData = new FormData();
    const appointmentJson = JSON.stringify(this.appointment);
    formData.append('model', appointmentJson);

    // Append optional file if selected
    if (this.selectedFile) {
      formData.append('file', this.selectedFile, this.selectedFile.name);
    }


    this.appointmentRequestService.SavePatientAppointment(formData).subscribe({
      next: (response) => {
        console.log('Appointment saved:', response);
        alert('Appointment saved successfully!');

        form.resetForm();
        this.appointment = {} as AppointmentRequestsModel;
        this.selectedFile = null;
      },
      error: (error) => {
        console.error('Error while saving appointment:', error);
        alert('Failed to save appointment. Please try again later.');
      }
    });
  }

onCancel(){
  this.router.navigate(['']); 
}


}
