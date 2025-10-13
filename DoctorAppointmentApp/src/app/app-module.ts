import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { NgSelectModule } from '@ng-select/ng-select';
import { DoctorsList } from './pages/doctors-list/doctors-list';
import { HttpClientModule } from '@angular/common/http';
import { Navbar } from './shared/navbar/navbar';
import { FormsModule } from '@angular/forms';
import { DoctorList } from './pages/admin/doctor-list/doctor-list';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { DoctorAddedit } from './pages/admin/doctor-addedit/doctor-addedit';
import { DoctorAvailability } from './pages/admin/doctor-availability/doctor-availability';

@NgModule({
  declarations: [
    App,
    DoctorsList,
    Navbar,
    DoctorList,
    DoctorAddedit,
    DoctorAvailability
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    NgbPaginationModule,
    NgMultiSelectDropDownModule.forRoot(),
    NgSelectModule
  ],
  providers: [
    provideBrowserGlobalErrorListeners()
  ],
  bootstrap: [App]
})
export class AppModule { }
