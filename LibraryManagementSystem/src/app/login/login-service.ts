import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environment/environment';
import { UserInfoModel } from '../../Models/UserInfoModel';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private http: HttpClient) {}

  login(model : UserInfoModel): Observable<any> {
    return this.http.post<any>(environment.apiBaseUrl + 'Accounts/Login' , model);
  }
 
  ValidateToken(): Observable<any> {
    return this.http.get<any>(environment.apiBaseUrl + 'Accounts/ValidateToken');
  }
 
  
}
