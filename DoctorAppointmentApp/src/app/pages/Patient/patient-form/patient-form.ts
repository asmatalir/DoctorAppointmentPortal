import { Component, ViewChild, TemplateRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PatientDetailsModel } from '../../../core/models/PatientDetailsModel';
import { NgForm } from '@angular/forms';
import { AppointmentRequestService } from '../../../core/services/appointment-request-service';
import { AppointmentRequestsModel } from '../../../core/models/AppointmentRequestsModel';
import { LocationService } from '../../../core/services/location-service';
import { ToastService } from '../../../core/services/toast-service';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';


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
  isExistingPatient: boolean = false;


  statesList: any[] = [];
  districtsList: any[] = [];
  talukasList: any[] = [];
  citiesList: any[] = [];

  appointment: AppointmentRequestsModel = new AppointmentRequestsModel();

  @ViewChild('patientForm') form?: NgForm;
  @ViewChild('unsavedChangesModal') unsavedChangesModal!: TemplateRef<any>;

  private formSaved = false;
  private modalRef: NgbModalRef;

  constructor(private route: ActivatedRoute,
    private appointmentRequestService: AppointmentRequestService,
    private locationService: LocationService,
    private toastService: ToastService,
    private router: Router,
    private modalService: NgbModal
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
      error: err => this.toastService.show("Error loading states", { classname: 'bg-danger text-white', delay: 1500 })

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
      error: err => this.toastService.show("Error loading districts", { classname: 'bg-danger text-white', delay: 1500 })
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
      error: err => this.toastService.show("Error loading talukas", { classname: 'bg-danger text-white', delay: 1500 })
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
      error: err => this.toastService.show("Error loading cities", { classname: 'bg-danger text-white', delay: 1500 })
    });
  }

  // User changes state
  onStateChange() {

    this.appointment.DistrictId = null;
    this.appointment.TalukaId = null;
    this.appointment.CityId = null;
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
      this.appointment.DistrictId = null; // reset selection
      return;
    }
    this.appointment.TalukaId = null;
    this.appointment.CityId = null;
    this.talukasList = [];
    this.citiesList = [];

    if (this.appointment.DistrictId) {
      this.loadTalukas(this.appointment.DistrictId);
    }
  }

  onTalukaChange() {

    if (!this.appointment.TalukaId) {
      this.toastService.show("Please select a District first.", { classname: 'bg-warning text-white', delay: 1500 });
      this.appointment.TalukaId = null;
      return;
    }
    this.appointment.CityId = null;
    this.citiesList = [];

    if (this.appointment.TalukaId) {
      this.loadCities(this.appointment.TalukaId);
    }
  }


  loadPatientDetails() {
    if (!this.appointment.AadhaarNumber)
       return;

    const enteredAadhaarNumber = this.appointment.AadhaarNumber;

    const doctorContext = {
      DoctorId: this.appointment.DoctorId,
      DoctorName: this.appointment.DoctorName,
      DoctorEmail: this.appointment.DoctorEmail,
      SlotId: this.appointment.SlotId,
      StartTime: this.appointment.StartTime,
      EndTime: this.appointment.EndTime,
      PreferredDate: this.appointment.PreferredDate,
      SelectedSpecializationId: this.appointment.SelectedSpecializationId
    };

    this.appointmentRequestService.GetPatientDetails(enteredAadhaarNumber)
      .subscribe({
        next: (response: AppointmentRequestsModel) => {

          this.isExistingPatient = response?.PatientId > 0;


          if (this.isExistingPatient) {
            this.appointment = { ...response };
            this.appointment.AadhaarNumber = response.AadhaarNumber || enteredAadhaarNumber;

            if (this.appointment.DateOfBirth) {
              this.appointment.DateOfBirth = new Date(this.appointment.DateOfBirth)
                .toISOString().substring(0, 10);
            } else {
              this.appointment.DateOfBirth = '';
            }

            this.loadDistricts(this.appointment.StateId!, this.appointment.DistrictId!);
          }

          else {
            this.appointment = new AppointmentRequestsModel();
            this.appointment.AadhaarNumber = enteredAadhaarNumber;
          }


          Object.assign(this.appointment, doctorContext);
        },

        error: (err) => {
          this.toastService.show(`Error: ${err?.error?.message || err?.error || err?.message}`,
            { classname: 'bg-danger text-white', delay: 1500 });
        }
      });
  }


  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      const maxSizeBytes = this.maxFileSizeMB * 1024 * 1024;

      if (file.size > maxSizeBytes) {
        this.toastService.show(`File size should not exceed ${this.maxFileSizeMB} MB.`, { classname: 'bg-warning text-white', delay: 1500 });
        event.target.value = '';
        this.selectedFile = null;
        return;
      }

      this.selectedFile = file;
    } else {
      this.selectedFile = null;
    }
  }

  canDeactivate(): Promise<boolean> | boolean {
    if (this.form?.dirty && !this.formSaved) {
      return new Promise((resolve) => {
        this.modalRef = this.modalService.open(this.unsavedChangesModal, { centered: true });
        this.modalRef.result.then(
          (result) => resolve(result === 'discard'),
          () => resolve(false)
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
      this.toastService.show("Please fill all required fields before submitting", { classname: 'bg-warning text-white', delay: 1500 })

      form.control.markAllAsTouched();
      return;
    }

    const age = this.calculateAge(this.appointment.DateOfBirth);
    if (age < 0 || age > 125) {
      this.toastService.show("Invalid Date of Birth. Patient age exceeds the allowed limit.", { classname: 'bg-warning text-white', delay: 1500 });
      return;
    }
    const formData = new FormData();
    const appointmentJson = JSON.stringify(this.appointment);
    formData.append('model', appointmentJson);

    if (this.selectedFile) {
      formData.append('file', this.selectedFile, this.selectedFile.name);
    }


    this.appointmentRequestService.SavePatientAppointment(formData).subscribe({
      next: (response) => {
        this.toastService.show("Appointment saved successfully", { classname: 'bg-success text-white', delay: 1500 })



        form.resetForm();
        this.formSaved = true;
        form.form.markAsPristine();
        this.appointment = {} as AppointmentRequestsModel;
        this.selectedFile = null;
        this.router.navigate(['']);
      },
      error: (error) => {
        this.toastService.show(`Error: ${error?.error?.message || error?.error || error?.message || "An unexpected error occurred."}`, { classname: 'bg-danger text-white', delay: 3000 });

      }
    });
  }

  onCancel() {
    this.router.navigate(['']);
  }


}
