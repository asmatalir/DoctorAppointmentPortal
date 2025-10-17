import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PatientDetailsModel } from '../../../core/models/PatientDetailsModel';
import { NgForm } from '@angular/forms';
import { AppointmentRequestService } from '../../../core/services/appointment-request-service';
import { AppointmentRequestsModel } from '../../../core/models/AppointmentRequestsModel';


@Component({
  selector: 'app-patient-form',
  standalone: false,
  templateUrl: './patient-form.html',
  styleUrl: './patient-form.scss'
})
export class PatientForm {

  doctorId!: number;
  doctorName : string;
  slotId!: number;
  startTime!: string;
  endTime!: string;
  slotDate!: string;
  speializationId : number;

  statesList: any[] = [];
  districtsList: any[] = [];
  talukasList: any[] = [];
  citiesList: any[] = [];

  appointment : AppointmentRequestsModel = new AppointmentRequestsModel();

  constructor(private route: ActivatedRoute, private appointmentRequestService : AppointmentRequestService) {}

  ngOnInit() {
    debugger;
    this.route.queryParams.subscribe(params => {
      this.appointment.DoctorId = +params['doctorId'];
      this.appointment.DoctorName = params['doctorName'];
      this.appointment.SlotId = +params['slotId'];
      this.appointment.StartTime = params['startTime'];
      this.appointment.EndTime = params['endTime'];
      this.appointment.PreferredDate = params['slotDate'];
      this.appointment.SelectedSpecializationId = params['specializationId'];
    });
  }
  loadPatientDetails() {
    if (!this.appointment.ContactNumber) {
      return; // Exit if contact number is empty
    }

    const enteredContact = this.appointment.ContactNumber;
    const doctorContext = {
      DoctorId: this.appointment.DoctorId,
      DoctorName: this.appointment.DoctorName,
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
          this.statesList = response.StatesList || [];
          this.districtsList = response.DistrictsList || [];
          this.talukasList = response.TalukasList || [];
          this.citiesList = response.CitiesList || [];

          if (this.appointment.DateOfBirth) {
            const dob = new Date(this.appointment.DateOfBirth);
            this.appointment.DateOfBirth = dob.toISOString().substring(0, 10); 
          } else {
            this.appointment.DateOfBirth = ''; // empty if null
          }
        },
        error: (err) => {
          console.error("Error loading patient details:", err);
        }
      });
  }
  onSubmit(form: NgForm) {
    if (form.invalid) {
      alert('Please fill all required fields before submitting.');
      return;
    }
  
    // Prepare model data (assuming your form is bound to a variable like `this.appointmentModel`)
  
    this.appointmentRequestService.SavePatientAppointment(this.appointment).subscribe({
      next: (response) => {
        console.log('Appointment saved:', response);
        alert('Appointment saved successfully!');
        
        form.resetForm();
        this.appointment = {} as AppointmentRequestsModel;
      },
      error: (error) => {
        console.error('Error while saving appointment:', error);
        alert('Failed to save appointment. Please try again later.');
      }
    });
  }
  


  
}
