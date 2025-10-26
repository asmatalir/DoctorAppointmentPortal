import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, GuardResult, MaybeAsync, RouterStateSnapshot,Router } from '@angular/router';
import { AuthService } from '../services/auth-service';
import { ToastService } from '../services/toast-service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(
    private router: Router,
    private authenticationService: AuthService,
    private toastService: ToastService
  ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
       const token = sessionStorage.getItem('token');

       debugger;
    // If no token or token is expired, redirect to login
    if (!token || this.authenticationService.isTokenExpired(token)) {
      sessionStorage.clear(); 
      this.router.navigate(['']);
      return false;
    }

    // Token is valid
    return true;
  }
}
