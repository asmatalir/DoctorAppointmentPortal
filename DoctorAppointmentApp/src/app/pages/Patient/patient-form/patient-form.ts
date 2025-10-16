import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PatientDetailsModel } from '../../../core/models/PatientDetailsModel';
import { NgForm } from '@angular/forms';
import { AppointmentRequestService } from '../../../core/services/appointment-request-service';


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

  patient : PatientDetailsModel = new PatientDetailsModel();

  constructor(private route: ActivatedRoute, private appointmentRequestService : AppointmentRequestService) {}

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.doctorId = +params['doctorId'];
      this.doctorName = params['doctorName'];
      this.slotId = +params['slotId'];
      this.startTime = params['startTime'];
      this.endTime = params['endTime'];
      this.slotDate = params['slotDate'];
      this.speializationId = params['specializationId'];
    });
  }
  loadPatientDetails() {
    if (!this.patient.ContactNumber) {
      return; // Exit if contact number is empty
    }
  
    const enteredContact = this.patient.ContactNumber;
    this.appointmentRequestService.GetPatientDetails(this.patient.ContactNumber)
      .subscribe({
        next: (response: PatientDetailsModel) => {
          // Assign the entire response to the patient model
          this.patient = response;
          this.patient.ContactNumber = response.ContactNumber || enteredContact;
          this.statesList = response.StatesList || [];
          this.districtsList = response.DistrictsList || [];
          this.talukasList = response.TalukasList || [];
          this.citiesList = response.CitiesList || [];

          if (this.patient.DateOfBirth) {
            const dob = new Date(this.patient.DateOfBirth);
            this.patient.DateOfBirth = dob.toISOString().substring(0, 10); 
          } else {
            this.patient.DateOfBirth = ''; // empty if null
          }
        },
        error: (err) => {
          console.error("Error loading patient details:", err);
        }
      });
  }
  onSubmit(form: NgForm)
  {

  }


  
}
