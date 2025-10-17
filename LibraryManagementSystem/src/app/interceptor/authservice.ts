import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class Authservice {
  constructor(private router: Router) {}

  logout(): void {
    sessionStorage.clear();
    this.router.navigate(['/books/login']);
  }

  getUsername(): string | null {
    const token = sessionStorage.getItem('jwtToken');
    if(!token)
    {
      sessionStorage.clear();
      return null;
    }
    return sessionStorage.getItem('username');
  }
}
