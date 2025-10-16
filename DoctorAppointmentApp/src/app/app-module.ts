import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { HttpClient, HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';


import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { NgSelectModule } from '@ng-select/ng-select';
import { DoctorsList } from './pages/Patient/doctors-list/doctors-list';
import { HttpClientModule } from '@angular/common/http';
import { Navbar } from './shared/navbar/navbar';
import { FormsModule } from '@angular/forms';
import { DoctorList } from './pages/admin/doctor-list/doctor-list';
import { NgbModal, NgbPaginationModule, NgbToastModule } from '@ng-bootstrap/ng-bootstrap';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { DoctorAddedit } from './pages/admin/doctor-addedit/doctor-addedit';
import { DoctorAvailability } from './pages/admin/doctor-availability/doctor-availability';
import { AppointmentRequests } from './pages/admin/appointment-requests/appointment-requests';
import { DoctorAppointmentRequests } from './pages/doctor/doctor-appointment-requests/doctor-appointment-requests';
import { LoaderInterceptor } from './core/helpers/loader-interceptor';
import { Login } from './account/login/login';
import { Loader } from './core/helpers/loader/loader';
import { ToastContainer } from './core/helpers/toast-container/toast-container';
import { AuthInterceptor } from './core/helpers/auth-interceptor';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { DoctorAvailableSlotsModal } from './pages/Patient/doctor-available-slots-modal/doctor-available-slots-modal';
import { PatientForm } from './pages/Patient/patient-form/patient-form';

@NgModule({
  declarations: [
    App,
    DoctorsList,
    Navbar,
    DoctorList,
    DoctorAddedit,
    DoctorAvailability,
    AppointmentRequests,
    DoctorAppointmentRequests,
    Login,
    Loader,
    ToastContainer,
    DoctorAvailableSlotsModal,
    PatientForm
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    NgbPaginationModule,
    NgMultiSelectDropDownModule.forRoot(),
    NgSelectModule,
    NgbDropdownModule,
    NgbToastModule,
    NgbModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: LoaderInterceptor, multi: true },
  ],
  bootstrap: [App]
})
export class AppModule { }
