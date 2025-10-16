import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DoctorList } from './pages/admin/doctor-list/doctor-list';
import { DoctorAddedit } from './pages/admin/doctor-addedit/doctor-addedit';
import { DoctorAvailability } from './pages/admin/doctor-availability/doctor-availability';
import { DoctorsList } from './pages/Patient/doctors-list/doctors-list';
import { AppointmentRequests } from './pages/admin/appointment-requests/appointment-requests';
import { DoctorAppointmentRequests } from './pages/doctor/doctor-appointment-requests/doctor-appointment-requests';
import { Login } from './account/login/login';
import { AuthGuard } from './core/guards/auth-guard';
import { PatientForm } from './pages/Patient/patient-form/patient-form';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' }, 
  { path: 'login', component: Login },
  { path: 'doctors', component: DoctorList ,canActivate: [AuthGuard]},     
  { path: 'doctorss', component: DoctorsList },
  { path: 'doctors/add', component: DoctorAddedit },
  { path: 'doctors/edit/:id', component: DoctorAddedit },
  { path: 'doctors/availability/:id', component: DoctorAvailability },
  { path: 'doctors/appointmentrequests', component: AppointmentRequests },
  { path: 'doctors/doctorappointmentrequests', component: DoctorAppointmentRequests },
  { path: 'patient/patientdetails', component: PatientForm },


  { path: '**', redirectTo: '/doctors' }                   
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
