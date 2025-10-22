import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Login } from './login';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbModule, NgbToastModule } from '@ng-bootstrap/ng-bootstrap';
import { LoginRoutingModule } from './login-routing.module';


@NgModule({
  declarations: [Login], // declare your component
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModule,
    NgbToastModule,
    LoginRoutingModule
     // for lazy loading
  ]
})
export class LoginModule { }
