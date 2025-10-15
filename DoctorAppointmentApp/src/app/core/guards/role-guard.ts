import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, GuardResult, MaybeAsync, RouterStateSnapshot,Router } from '@angular/router';
import { AuthService } from '../services/auth-service';
import { ToastService } from '../services/toast-service';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {
  constructor(
    private router: Router,
    private toastService: ToastService,
    private authenticationService: AuthService
  ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const requiredRoles = route.data?.['roles'] as string[] || [];
    if (this.authenticationService.hasRole(requiredRoles)) {
      return true;
    } else {
      this.toastService.show("You do not have access to that page", { classname: 'bg-danger text-white', delay: 2000 });
      this.router.navigate(['/search-book']);  //redirect to a default page/ dashboard
      return false;
    }
  }
  
}
