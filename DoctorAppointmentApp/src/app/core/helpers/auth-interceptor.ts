// src/app/interceptors/auth.interceptor.ts
import { DebugElement, Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { ToastService } from '../services/toast-service';

@Injectable()

export class AuthInterceptor implements HttpInterceptor {
  constructor(
    private router: Router,
    private toastService : ToastService
  ) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = sessionStorage.getItem('token');
    if (token) {
      req = req.clone({ setHeaders: { Authorization: `Bearer ${token}` }});
    }
    return next.handle(req).pipe(
      catchError((err: HttpErrorResponse) => {
        if (err.status === 401) {
          this.toastService.show("Unauthorized access", { classname: 'bg-danger text-white', delay: 1500 });
          this.router.navigate(['']);
          (err as any).isAuthError = true;
        }
        return throwError(() => err);
      })
    );
  }
}
