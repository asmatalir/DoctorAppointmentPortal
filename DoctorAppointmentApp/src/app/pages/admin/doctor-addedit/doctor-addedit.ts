import { Component } from '@angular/core';
import { DoctorsService } from '../../../core/services/doctors-service';
import { IDropdownSettings } from 'ng-multiselect-dropdown';

interface DoctorSession {
  StartTime: string;
  EndTime: string;
  Duration: number | null;
}

interface DoctorAvailability {
  DayOfWeek: string;
  Sessions: DoctorSession[];
}

interface Doctor {
  DoctorId?: number;
  FirstName?: string;
  LastName?: string;
  Gender?: string;
  DateOfBirth?: string;
  ContactNumber?: string;
  Email?: string;
  UserName?: string;
  Password?: string;
  ExperienceYears?: number;
  ConsultationFees?: number;
  HospitalName?: string;
  Specializations?: number[];
  Qualifications?: number[];
  Rating?: number;
  Description?: string;
  StateId?: number;
  DistrictId?: number;
  TalukaId?: number;
  CityId?: number;
  AddressLine?: string;
  Pincode?: string;
  DoctorAvailabilityList: DoctorAvailability[];
}


@Component({
  selector: 'app-doctor-addedit',
  standalone: false,
  templateUrl: './doctor-addedit.html',
  styleUrl: './doctor-addedit.scss'
})
export class DoctorAddedit {
  
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
 doctor: Doctor = {
    DoctorAvailabilityList: []
  };

  // Example dropdown data
  daysOfWeek = [
    { name: 'Monday', value: 'Monday' },
    { name: 'Tuesday', value: 'Tuesday' },
    { name: 'Wednesday', value: 'Wednesday' },
    { name: 'Thursday', value: 'Thursday' },
    { name: 'Friday', value: 'Friday' },
    { name: 'Saturday', value: 'Saturday' },
    { name: 'Sunday', value: 'Sunday' }
  ];

  ngOnInit(): void { }

  // -------------------------
  // Availability & Session logic
  // -------------------------

  addAvailability() {
    this.doctor.DoctorAvailabilityList.push({
      DayOfWeek: '',
      Sessions: [
        { StartTime: '', EndTime: '', Duration: null }
      ]
    });
  }

  removeAvailability(index: number) {
    this.doctor.DoctorAvailabilityList.splice(index, 1);
  }

  addSession(dayIndex: number) {
    this.doctor.DoctorAvailabilityList[dayIndex].Sessions.push({
      StartTime: '',
      EndTime: '',
      Duration: null
    });
  }

  removeSession(dayIndex: number, sessionIndex: number) {
    this.doctor.DoctorAvailabilityList[dayIndex].Sessions.splice(sessionIndex, 1);
  }



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
