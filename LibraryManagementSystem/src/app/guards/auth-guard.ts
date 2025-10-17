import { inject } from '@angular/core';
import { CanActivateFn,Router } from '@angular/router';
import { catchError, map, of } from 'rxjs';
import { LoginService } from '../login/login-service';

export const authGuard: CanActivateFn = (route, state) => {
  debugger;
  const router = inject(Router);
  const loginService = inject(LoginService);
  const token = sessionStorage.getItem('jwtToken');

  if (!token) {
    router.navigate(['/books/login']);
    return of(false);
  }

  return loginService.ValidateToken().pipe(
    map(() => true),
    catchError(() => {
      router.navigate(['/books/login']);
      return of(false);
    })
  );
};
