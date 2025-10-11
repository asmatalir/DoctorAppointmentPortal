import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DoctorList } from './pages/admin/doctor-list/doctor-list';
import { DoctorAddedit } from './pages/admin/doctor-addedit/doctor-addedit';

const routes: Routes = [
  { path: '', redirectTo: '/doctors', pathMatch: 'full' }, 
  { path: 'doctors', component: DoctorList },     
  { path: 'doctors/add', component: DoctorAddedit },
  { path: 'doctors/edit/:id', component: DoctorAddedit },
  { path: '**', redirectTo: '/doctors' }                   
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
