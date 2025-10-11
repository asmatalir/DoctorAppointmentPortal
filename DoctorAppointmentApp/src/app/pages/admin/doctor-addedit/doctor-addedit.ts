import { Component } from '@angular/core';
import { DoctorsService } from '../../../core/services/doctors-service';
import { IDropdownSettings } from 'ng-multiselect-dropdown';


@Component({
  selector: 'app-doctor-addedit',
  standalone: false,
  templateUrl: './doctor-addedit.html',
  styleUrl: './doctor-addedit.scss'
})
export class DoctorAddedit {
  doctor: any = {}; // Object bound to form
  
specializationsList = [
  { SpecializationId: 1, SpecializationName: 'Cardiology' },
  { SpecializationId: 2, SpecializationName: 'Dermatology' },
  { SpecializationId: 3, SpecializationName: 'Neurology' },
  // Add more
];

qualificationsList = [
  { QualificationId: 1, QualificationName: 'MBBS' },
  { QualificationId: 2, QualificationName: 'MD' },
  { QualificationId: 3, QualificationName: 'DO' },
  // Add more
];
// In your component.ts
daysOfWeek = [
  { name: 'Sunday', value: 0 },
  { name: 'Monday', value: 1 },
  { name: 'Tuesday', value: 2 },
  { name: 'Wednesday', value: 3 },
  { name: 'Thursday', value: 4 },
  { name: 'Friday', value: 5 },
  { name: 'Saturday', value: 6 }
];


  constructor(private doctorService: DoctorsService) {}
  dropdownSettings: IDropdownSettings = {
    singleSelection: false,
    idField: 'PublisherId',
    textField: 'PublisherName',
    selectAllText: 'Select All',
    unSelectAllText: 'Unselect All',
    itemsShowLimit: 2,
    allowSearchFilter: true
  };
  onSubmit(form: any) {
    if (form.valid) {
      console.log('Form Data:', this.doctor);
      // Call backend API to save doctor
    } else {
      console.log('Form is invalid');
    }
  }
}
