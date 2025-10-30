import { Injectable } from '@angular/core';
import { UserProfilesModel } from '../models/UserProfilesModel';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';



@Injectable({
  providedIn: 'root'
})
export class LoginService {
  constructor(private apiService : ApiService) {}

login(model: UserProfilesModel): Observable<any> {
  return this.apiService.post<any>('Accounts/login', model);
}

ValidateToken(): Observable<any> {
  return this.apiService.get<any>('Accounts/ValidateToken');
}
 
}
