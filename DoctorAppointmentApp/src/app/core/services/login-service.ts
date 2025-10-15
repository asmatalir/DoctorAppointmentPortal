import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserProfilesModel } from '../models/UserProfilesModel';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';



@Injectable({
  providedIn: 'root'
})
export class LoginService {
  constructor(private http: HttpClient) {}

  login(model : UserProfilesModel): Observable<any> {
    return this.http.post<any>(environment.apiBaseUrl + 'Accounts/login' , model);
  }
 
  ValidateToken(): Observable<any> {
    return this.http.get<any>(environment.apiBaseUrl + 'Accounts/ValidateToken');
  }
 
}
