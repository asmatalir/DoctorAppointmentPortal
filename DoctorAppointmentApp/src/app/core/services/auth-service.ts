import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  getRoles(): string[] {
    const token = sessionStorage.getItem('token');
    if (!token) return [];

    try {


      const decoded: any = jwtDecode(token);
      // used in pc
      // return decoded.role ? (Array.isArray(decoded.role) ? decoded.role : [decoded.role]) : [];

      //used in laptop
      const roleClaim = decoded.role || decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
      return roleClaim ? Array.isArray(roleClaim) ? roleClaim : [roleClaim] : [];
    } catch {
      return [];
    }
  }

  hasRole(requiredRoles: string[]): boolean {
    debugger;
    const userRoles = this.getRoles();
    return requiredRoles.some(r => userRoles.includes(r));
  }


  isTokenExpired(token: string): boolean {
    try {
      const decoded: any = jwtDecode(token);
      if (!decoded.exp) return true;

      const expiryTime = decoded.exp * 1000;
      return Date.now() > expiryTime;
    } catch (e) {
      return true;
    }
  }
}
