import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth-guard';
import { Navbar } from './shared/navbar/navbar';
import { MainLayout } from './shared/main-layout/main-layout';
import { RoleGuard } from './core/guards/role-guard';
import { Homepage } from './shared/homepage/homepage';

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
    children: [
      { path: '', component: Homepage },
      { path: 'admin', loadChildren: () => import('./pages/admin/admin.module').then(m => m.AdminModule), canActivate: [AuthGuard, RoleGuard], data: { roles: ['Admin'] } },
      { path: 'doctor', loadChildren: () => import('./pages/doctor/doctor.module').then(m => m.DoctorModule), canActivate: [AuthGuard, RoleGuard], data: { roles: ['Doctor'] } },
      { path: 'patient', loadChildren: () => import('./pages/Patient/patient.module').then(m => m.PatientModule) }
    ]
  },

  // wildcard fallback
  { path: '**', redirectTo: '' }
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
