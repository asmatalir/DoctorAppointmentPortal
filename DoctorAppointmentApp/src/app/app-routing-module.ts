import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DoctorList } from './pages/admin/doctor-list/doctor-list';
import { DoctorAddedit } from './pages/admin/doctor-addedit/doctor-addedit';
import { DoctorAvailability } from './pages/admin/doctor-availability/doctor-availability';
import { DoctorsList } from './pages/doctors-list/doctors-list';
import { AppointmentRequests } from './pages/admin/appointment-requests/appointment-requests';

const routes: Routes = [
  { path: '', redirectTo: '/doctors', pathMatch: 'full' }, 
  { path: 'doctors', component: DoctorList },     
  { path: 'doctorss', component: DoctorsList },
  { path: 'doctors/add', component: DoctorAddedit },
  { path: 'doctors/edit/:id', component: DoctorAddedit },
  { path: 'doctors/availability/:id', component: DoctorAvailability },
  { path: 'doctors/appointmentrequests', component: AppointmentRequests },
  { path: '**', redirectTo: '/doctors' }                   
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
