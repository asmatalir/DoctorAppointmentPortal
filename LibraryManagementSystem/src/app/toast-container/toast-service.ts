import { Injectable } from '@angular/core';

export type ToastType = 'success' | 'error' | 'info' | 'warning';

export interface AppToast {
  id: number;
  message: string;
  type?: ToastType;
  header?: string;
  delay?: number;
}
@Injectable({ providedIn: 'root' })
export class ToastService {
  private toasts: AppToast[] = [];
  private id = 0;

  getAll() { return this.toasts; }

  show(message: string, opts: Partial<AppToast> = {}) {
    const t: AppToast = { id: ++this.id, message, delay: 2500, ...opts };
    this.toasts = [...this.toasts, t];
  }
  success(msg: string, opts: Partial<AppToast> = {}) {
    this.show(msg, { type: 'success', ...opts });
  }
  error(msg: string, opts: Partial<AppToast> = {}) {
    this.show(msg, { type: 'error', ...opts });
  }
  info(msg: string, opts: Partial<AppToast> = {}) {
    this.show(msg, { type: 'info', ...opts });
  }
  warning(msg: string, opts: Partial<AppToast> = {}) {
    this.show(msg, { type: 'warning', ...opts });
  }

  remove(id: number) {
    this.toasts = this.toasts.filter(t => t.id !== id);
  }
  clear() { this.toasts = []; }
}
