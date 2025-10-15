import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { AppointmentRequestsModel } from '../models/AppointmentRequestsModel';

@Injectable({
  providedIn: 'root'
})
export class AppointmentRequestService {
    constructor(private http: HttpClient) { }
  
    AppointmentRequestGetList(model: AppointmentRequestsModel): Observable<AppointmentRequestsModel[]> {
      return this.http.post<AppointmentRequestsModel[]>(environment.apiBaseUrl + 'AppointmentRequests/AppointmentRequestsGetLists',model);
    }

    DoctorAppointmentRequestGetList(model: AppointmentRequestsModel): Observable<AppointmentRequestsModel[]> {
      return this.http.post<AppointmentRequestsModel[]>(environment.apiBaseUrl + 'AppointmentRequests/DoctorApppointmentGetLists',model);
    }

    UpdateStatus(model : AppointmentRequestsModel): Observable<any> {
      return this.http.post<any>( environment.apiBaseUrl + 'AppointmentRequests/DoctorAppointmentUpdateStatus',model);
    }
    
}
