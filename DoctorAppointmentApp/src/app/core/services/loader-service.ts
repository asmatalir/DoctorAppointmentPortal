import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoaderService {
  isLoading = new BehaviorSubject<boolean>(false);
  private requestCount : number = 0;

  show() {
    this.requestCount++;
    this.isLoading.next(true);
  }

  hide() {
    this.requestCount--;
    if (this.requestCount == 0) {
      this.isLoading.next(false);
    }
  }
}
