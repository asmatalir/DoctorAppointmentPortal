import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth-guard';
import { Navbar } from './shared/navbar/navbar';
import { MainLayout } from './shared/main-layout/main-layout';

const routes: Routes = [
  // standalone login page
  { 
    path: 'login', 
    loadChildren: () => import('./account/login/login.module').then(m => m.LoginModule) 
  },

  // main layout with navbar
  {
    path: '',
    component: MainLayout,
    canActivate : [AuthGuard], // MainLayoutComponent
    children: [
      { path: 'admin', loadChildren: () => import('./pages/admin/admin.module').then(m => m.AdminModule) },
      { path: 'doctor', loadChildren: () => import('./pages/doctor/doctor.module').then(m => m.DoctorModule) },
      { path: 'patient', loadChildren: () => import('./pages/Patient/patient.module').then(m => m.PatientModule) }
    ]
  },

  // wildcard fallback
  { path: '**', redirectTo: '/login' }
];



@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
