import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DoctorAppointmentRequests } from './doctor-appointment-requests/doctor-appointment-requests';
import { DoctorRoutingModule } from './doctor-routing.module';
import { NgbDropdown, NgbDropdownModule, NgbModule, NgbPaginationModule, NgbToastModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
  declarations: [
    DoctorAppointmentRequests
  ],
  imports: [
    CommonModule,
    DoctorRoutingModule,
    FormsModule,
    NgbModule,
    NgbToastModule,
    NgSelectModule,
    NgbPaginationModule,
    NgbDropdownModule,
    SharedModule
    
  ]
})
export class DoctorModule {}
