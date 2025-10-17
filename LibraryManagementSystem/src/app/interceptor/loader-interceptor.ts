import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable,throwError } from 'rxjs';
import { finalize,catchError } from 'rxjs/operators';
import { LoaderService } from '../loader/loader-service';
import { ToastService } from '../toast-container/toast-service';
import { Router } from '@angular/router';

@Injectable()
export class LoaderInterceptor implements HttpInterceptor {
  private toastShownRecently = false;
  constructor(private loaderService: LoaderService,
     private toast : ToastService ,
    private router : Router) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.loaderService.show();

    return next.handle(req).pipe(
      catchError((err: HttpErrorResponse) => {
        if (!this.toastShownRecently) {
          this.toastShownRecently = true;
          const msg = this.buildErrorMessage(err);
          this.toast.error(msg);
          setTimeout(() => this.toastShownRecently = false, 2000);
        }
        // this.router.navigate(['/books/login']);
        return throwError(() => err);
      }),
      finalize(() => this.loaderService.hide())
    );
  }

  private buildErrorMessage(err: HttpErrorResponse): string {
    if (err.status === 0)
      return 'Network error: please check your connection.';
    if (err.status === 400)
      return (err.error?.message ?? 'Bad request.');
    if (err.status === 401)
      return 'Your session has expired. Please log in again.';
    if (err.status === 403)
      return 'You do not have permission to perform this action.';
    if (err.status === 404)
      return 'Resource not found.';
    if (err.status >= 500)
      return 'Server error. Please try again later.';
    return err.message || 'Request failed.';
  }
}

