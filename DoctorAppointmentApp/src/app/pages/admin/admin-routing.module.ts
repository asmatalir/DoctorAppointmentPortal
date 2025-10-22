import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppointmentRequests } from './appointment-requests/appointment-requests';
import { DoctorAddedit } from './doctor-addedit/doctor-addedit';
import { DoctorAvailability } from './doctor-availability/doctor-availability';

const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: AppointmentRequests },
  { path: 'add-doctor', component: DoctorAddedit },
  { path: 'edit-doctor/:id', component: DoctorAddedit }, 
  { path: 'doctor-availability', component: DoctorAvailability },
  { path: 'edit-availability/:id', component: DoctorAvailability } 
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
