import { Component } from '@angular/core';
import { UserProfilesModel } from '../../core/models/UserProfilesModel';
import { LoginService } from '../../core/services/login-service';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { ToastService } from '../../core/services/toast-service';
import { UserService } from '../../core/services/user-service';


@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login {
  model: UserProfilesModel = new UserProfilesModel();


  constructor(private loginService: LoginService,
    private router: Router,
    private toastService: ToastService,
    private userService: UserService) { }

  ngOnInit() {
    this.model.UserName = "asmatali123";
    this.model.EnteredPassword = "Asmatali@123";
  }

  onClick(form: NgForm): void {

    if (form.invalid) {
        this.toastService.show("Please fill the required fields", { classname: 'bg-warning text-white', delay: 3000 });
      return;
    }
    debugger;
    this.loginService.login(this.model).subscribe((data: any) => {
      if (data.success) {
         debugger;
        
        this.toastService.show("Login Successful", { classname: 'bg-success text-white', delay: 3000 });
        this.userService.setUser({ username: data.username, role: data.userrole });
        sessionStorage.setItem('token', data.token);


          if (data.userrole === 'Admin') {
            this.router.navigate(['/admin/appointment-requests']);  // Admin default page
          } else if (data.userrole === 'Doctor') {
            this.router.navigate(['/doctor/appointment-requests']);  // Doctor default page
          } else {
            this.router.navigate(['']); 
          }

      } else {
        debugger;
        this.toastService.show(data.message, { classname: 'bg-danger text-white', delay: 3000 });
      }
    });
  }
  
}
