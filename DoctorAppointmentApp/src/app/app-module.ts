import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { DoctorsList } from './pages/doctors-list/doctors-list';
import { HttpClientModule } from '@angular/common/http';
import { Navbar } from './shared/navbar/navbar';

@NgModule({
  declarations: [
    App,
    DoctorsList,
    Navbar
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [
    provideBrowserGlobalErrorListeners()
  ],
  bootstrap: [App]
})
export class AppModule { }
