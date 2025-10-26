import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DoctorsList } from './doctors-list/doctors-list';
// import { DoctorAvailableSlotsModal } from './doctor-available-slots-modal/doctor-available-slots-modal';
import { PatientForm } from './patient-form/patient-form';
import { PatientRoutingModule } from './patient-routing.module';
import { NgbDropdownModule, NgbModule, NgbPaginationModule, NgbToastModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
  declarations: [
    DoctorsList,
    PatientForm
  ],
  imports: [
    CommonModule,
    PatientRoutingModule,
    FormsModule,
    NgbModule,
    SharedModule,
    NgbToastModule,
    NgSelectModule,
    NgbDropdownModule,
    NgbPaginationModule
  ]
})
export class PatientModule {}
