import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { HttpClient, HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';


import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { NgSelectModule } from '@ng-select/ng-select';
import { HttpClientModule } from '@angular/common/http';
import { Navbar } from './shared/navbar/navbar';
import { FormsModule } from '@angular/forms';
import { NgbModal, NgbPaginationModule, NgbToastModule } from '@ng-bootstrap/ng-bootstrap';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { LoaderInterceptor } from './core/helpers/loader-interceptor';
import { Login } from './account/login/login';
import { Loader } from './core/helpers/loader/loader';
import { ToastContainer } from './core/helpers/toast-container/toast-container';
import { AuthInterceptor } from './core/helpers/auth-interceptor';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { MainLayout } from './shared/main-layout/main-layout';


@NgModule({
  declarations: [
    App,
    Navbar,
    Loader,
    ToastContainer,
    MainLayout,
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
