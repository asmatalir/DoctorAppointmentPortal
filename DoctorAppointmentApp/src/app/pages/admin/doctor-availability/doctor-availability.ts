import { Component } from '@angular/core';
import { DoctorAvailabilitiesModel } from '../../../core/models/DoctorAvailabilitiesModel';
import { DoctorSessionsModel } from '../../../core/models/DoctorSessionsModel';
import { DoctorsModel } from '../../../core/models/DoctorsModel';
import { ActivatedRoute } from '@angular/router';
import { DoctorsService } from '../../../core/services/doctors-service';

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
    this.doctorService.GetDoctorDetails(id).subscribe({
      next: (res: any) => {
        if (res) {
          this.doctor = res; // directly assign the model if the structure matches
          
          // convert comma-separated strings to arrays if needed
          this.doctor.Specializations = res.SpecializationNames?.split(',') || [];
          this.doctor.Qualifications = res.QualificationNames?.split(',') || [];

          // ensure DoctorAvailabilityList exists
          this.doctor.DoctorAvailabilityList = res.DoctorAvailabilityList || [];
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
  


}
