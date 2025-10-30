import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppointmentRequests } from './appointment-requests/appointment-requests';
import { DoctorAddedit } from './doctor-addedit/doctor-addedit';
import { DoctorAvailability } from './doctor-availability/doctor-availability';
import { DoctorList } from './doctor-list/doctor-list';
import { pendingChangesGuard } from '../../core/guards/pending-changes-guard-guard';

const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'appointment-requests', component: AppointmentRequests },
  { path: 'doctor-list', component: DoctorList },
  { path: 'add-doctor', component: DoctorAddedit,canDeactivate: [pendingChangesGuard] },
  { path: 'edit-doctor/:id', component: DoctorAddedit,canDeactivate: [pendingChangesGuard] }, 
  { path: 'doctor-availability', component: DoctorAvailability },
  { path: 'edit-availability/:id', component: DoctorAvailability } 
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
