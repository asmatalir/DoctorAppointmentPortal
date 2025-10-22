import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DoctorAppointmentRequests } from './doctor-appointment-requests/doctor-appointment-requests';

const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'appointment-requests', component: DoctorAppointmentRequests },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DoctorRoutingModule {}
