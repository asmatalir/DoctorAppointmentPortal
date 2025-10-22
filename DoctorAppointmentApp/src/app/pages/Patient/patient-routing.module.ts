import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DoctorsList } from './doctors-list/doctors-list';
import { DoctorAvailableSlotsModal } from './doctor-available-slots-modal/doctor-available-slots-modal';
import { PatientForm } from './patient-form/patient-form';

const routes: Routes = [
  { path: '', redirectTo: 'doctor-list', pathMatch: 'full' },
  { path: 'doctor-list', component: DoctorsList },
  { path: 'doctor-available-slots', component: DoctorAvailableSlotsModal },
  { path: 'patient-form', component: PatientForm },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PatientRoutingModule {}
