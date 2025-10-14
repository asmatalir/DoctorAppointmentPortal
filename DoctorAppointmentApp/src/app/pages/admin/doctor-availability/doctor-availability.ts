import { Component } from '@angular/core';
import { DoctorAvailabilitiesModel } from '../../../core/models/DoctorAvailabilitiesModel';
import { DoctorSessionsModel } from '../../../core/models/DoctorSessionsModel';
import { DoctorsModel } from '../../../core/models/DoctorsModel';
import { ActivatedRoute } from '@angular/router';
import { DoctorsService } from '../../../core/services/doctors-service';
import { DoctorUnavailabilityModel } from '../../../core/models/DoctorUnavailablityModel';

@Component({
  selector: 'app-doctor-availability',
  standalone: false,
  templateUrl: './doctor-availability.html',
  styleUrl: './doctor-availability.scss'
})
export class DoctorAvailability {

  doctor: DoctorsModel = new DoctorsModel();
  

constructor(
  private doctorService: DoctorsService,
  private route: ActivatedRoute
) { }

ngOnInit(): void {
  const doctorId = Number(this.route.snapshot.paramMap.get('id')); 
  if (doctorId) {
    this.loadDoctorDetails(doctorId);
  }
}

  daysOfWeek = [
    { name: 'Monday', value: 1 },
    { name: 'Tuesday', value: 2 },
    { name: 'Wednesday', value: 3 },
    { name: 'Thursday', value: 4 },
    { name: 'Friday', value: 5 },
    { name: 'Saturday', value: 6 },
    { name: 'Sunday', value: 7 }
  ];


  loadDoctorDetails(id: number) {
    this.doctorService.GetDoctorAvailabilityDetails(id).subscribe({
      next: (res: any) => {
        if (res) {
          this.doctor = res;
          
          // convert comma-separated strings to arrays if needed
          this.doctor.Specializations = res.SpecializationNames?.split(',') || [];
          this.doctor.Qualifications = res.QualificationNames?.split(',') || [];

          // ensure DoctorAvailabilityList exists
          this.doctor.DoctorAvailabilityList = res.DoctorAvailabilityList || [];
          this.doctor.DoctorAvailabilityExceptionsList = res.DoctorAvailabilityExceptionsList || [];
          // Inside your component, after fetching data
          this.doctor.DoctorAvailabilityExceptionsList.forEach(ex => {
            if (ex.ExceptionDate) {
                ex.ExceptionDate = new Date(ex.ExceptionDate).toISOString().split('T')[0];
            }
          });

        }
      },
      error: (err) => console.error('Error loading doctor details', err)
    });
  }

 
  addAvailability() {
    this.doctor.DoctorAvailabilityList.push({DayOfWeek: '',StartTime: '',EndTime: '',Duration : 0} as DoctorAvailabilitiesModel);
  }
  
  // Remove a specific availability slot
  removeAvailability(index: number) {
    this.doctor.DoctorAvailabilityList.splice(index, 1);
  }

  SaveDoctorAvailabilities() {
    if (!this.doctor.DoctorId) {
      console.error('DoctorId is missing!');
      return;
    }
  
    // Send the full DoctorsModel
    this.doctorService.saveDoctorAvailability(this.doctor).subscribe({
      next: (res) => {
        console.log('Availabilities saved successfully', res);
        alert('Doctor availability saved successfully!');
      },
      error: (err) => {
        console.error('Error saving availabilities', err);
        alert('Error saving availability!');
      }
    });
  }
  

  addUnavailability() {
    if (!this.doctor.DoctorAvailabilityExceptionsList) {
      this.doctor.DoctorAvailabilityExceptionsList = [];
    }
  
    this.doctor.DoctorAvailabilityExceptionsList.push({
      UnavailabilityId: 0,
      DoctorId: this.doctor.DoctorId,
      ExceptionDate: '',
      StartTime: '',
      EndTime: '',
      Reason: ''
    } as DoctorUnavailabilityModel);
  }
  
  
  

  removeUnavailability(index: number) {
    this.doctor.DoctorAvailabilityExceptionsList.splice(index, 1);
  }


}
