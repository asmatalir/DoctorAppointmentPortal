// import { Component } from '@angular/core';

// @Component({
//   selector: 'app-toast-container',
//   standalone: false,
//   templateUrl: './toast-container.html',
//   styleUrl: './toast-container.css'
// })
// export class ToastContainer {

// }
import { Component } from '@angular/core';
import { ToastService,AppToast } from './toast-service';
import { NgbToastModule } from '@ng-bootstrap/ng-bootstrap';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-toasts',
  standalone: false,
  templateUrl: './toast-container.html',
  styleUrl: './toast-container.css',
})
export class ToastContainer {
  constructor(public toastSvc: ToastService) {}
  trackById = (_: number, t: any) => t.id;
}

