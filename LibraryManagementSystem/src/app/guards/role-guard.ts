import { inject } from '@angular/core';
import { CanActivateFn,Router } from '@angular/router';
import { ToastService } from '../toast-container/toast-service';
import { AuthService } from './auth-service';

export const roleGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const toaster = inject(ToastService);
  const authService = inject(AuthService);

  const requiredRoles = route.data?.['roles'] as string[] || [];

  if (authService.hasRole(requiredRoles)) {
    return true;
  } else {
    toaster.error("You do not have access to that page", { delay: 2000 });
    router.navigate(['/books']);
    return false;
  }
};
