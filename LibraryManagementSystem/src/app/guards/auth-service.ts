import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  getRoles(): string[] {
    const token = sessionStorage.getItem('jwtToken');
    if (!token) return [];

    try {
      const decoded: any = jwtDecode(token);
      return decoded.role ? (Array.isArray(decoded.role) ? decoded.role : [decoded.role]) : [];
    } catch {
      return [];
    }
  }

  hasRole(requiredRoles: string[]): boolean {
    const userRoles = this.getRoles();
    return requiredRoles.some(r => userRoles.includes(r));
  }
}
