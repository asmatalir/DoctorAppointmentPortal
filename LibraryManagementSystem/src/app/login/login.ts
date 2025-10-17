import { Component } from '@angular/core';
import { LoginService } from './login-service';
import { UserInfoModel } from '../../Models/UserInfoModel';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { ToastService } from '../toast-container/toast-service';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {

  model: UserInfoModel = new UserInfoModel();


  constructor(private loginService: LoginService, private router: Router, private toast : ToastService) { }

  onClick(form: NgForm): void {

    if (form.invalid) {
      this.toast.warning("Please fill the required fields.");
      return;
    }

    this.loginService.login(this.model).subscribe({
      next: response => {
        if (response.success) {
          this.toast.success('Login Successfull');
            sessionStorage.setItem('jwtToken', response.token);
            sessionStorage.setItem('username', response.username);
            this.router.navigate(['/books']);
        } else {
         this.toast.error(response.message);
        }
      },
      error: err => {
        this.toast.error('Login failed');
      }
    });
  }

}
