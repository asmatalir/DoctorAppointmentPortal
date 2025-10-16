import { Component } from '@angular/core';
import { UserProfilesModel } from '../../core/models/UserProfilesModel';
import { LoginService } from '../../core/services/login-service';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { ToastService } from '../../core/services/toast-service';


@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login {
  model: UserProfilesModel = new UserProfilesModel();


  constructor(private loginService: LoginService, private router: Router,private toastService : ToastService) { }
 
  ngOnInit(){
    this.model.UserName="asmatali123";
    this.model.EnteredPassword="Asmatali@123";
  }

  onClick(form: NgForm): void {

    if (form.invalid) {
      // this.toast.warning("Please fill the required fields.");
      return;
    }

    this.loginService.login(this.model).subscribe((data:any) => { 
      if(data.success){
        const redirectUrl = sessionStorage.getItem('redirectUrl');
        this.toastService.show("Login Successful", { classname: 'bg-success text-white', delay: 3000 });
        sessionStorage.setItem('currentUser', JSON.stringify({ username: data.username,role : data.userrole }));
        sessionStorage.setItem('token', data.token);
        sessionStorage.removeItem('sessionExpiredNotified');
        sessionStorage.removeItem('redirectUrl');
        if (redirectUrl) {
          this.router.navigateByUrl(redirectUrl);  
        } else {
          this.router.navigate(['/doctors']);   
        }
      } else {
        this.toastService.show(data.message, { classname: 'bg-danger text-white', delay: 3000 });
      }
    });
  }
}
