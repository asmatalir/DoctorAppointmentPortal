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
    // Case 1: No token (first visit or logged out)
    if (!token) {
      this.handleRedirect(state, false);  // no toast
      return false;
    }
  
    // Case 2: Token exists but is expired
    if (this.authenticationService.isTokenExpired(token)) {
      sessionStorage.removeItem('token');
      this.handleRedirect(state, true);   // show toast
      return false;
    }
  
    // Case 3: Token is valid
    return true;
  }

  private handleRedirect(state: RouterStateSnapshot, showToast: boolean) {
    // Store redirect URL if user tried to access a protected route
    if (state.url && state.url !== '/auth/login' && state.url !== '/') {
      sessionStorage.setItem('redirectUrl', state.url);
    }
  
    // Optional toast message
    if (showToast) {
      this.toastService.show(
        'Your session has expired. Please login again.',
        { classname: 'bg-danger text-white', delay: 3000 }
      );
    }
  
    // Redirect to login
    this.router.navigate(['/auth/login']);
  }
  
}
