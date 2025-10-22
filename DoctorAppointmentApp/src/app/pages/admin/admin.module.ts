import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppointmentRequests } from './appointment-requests/appointment-requests';
import { DoctorAddedit } from './doctor-addedit/doctor-addedit';
import { DoctorAvailability } from './doctor-availability/doctor-availability';
import { AdminRoutingModule } from './admin-routing.module';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgbDropdownMenu, NgbDropdownModule, NgbModule, NgbPaginationModule, NgbToastModule } from '@ng-bootstrap/ng-bootstrap';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { FormsModule } from '@angular/forms';
import { DoctorList } from './doctor-list/doctor-list';

// import { Navbar } from '../../shared/navbar/navbar';

@NgModule({
  declarations: [
    AppointmentRequests,
    DoctorAddedit,
    DoctorAvailability,
    DoctorList
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    FormsModule,
    NgbModule,
    NgbToastModule,
    NgSelectModule,
    NgbPaginationModule,
    NgbDropdownModule,

  ]
})
export class AdminModule { }
