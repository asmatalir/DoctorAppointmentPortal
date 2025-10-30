import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AppointmentRequestsModel } from '../models/AppointmentRequestsModel';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class AppointmentRequestService {
    constructor(private apiService : ApiService) { }
  
  AppointmentRequestGetList(model: AppointmentRequestsModel): Observable<AppointmentRequestsModel[]> {
    return this.apiService.post<AppointmentRequestsModel[]>('AppointmentRequests/AppointmentRequestsGetLists', model);
  }

  DoctorAppointmentRequestGetList(model: AppointmentRequestsModel): Observable<AppointmentRequestsModel[]> {
    return this.apiService.post<AppointmentRequestsModel[]>('AppointmentRequests/DoctorApppointmentGetLists', model);
  }

  UpdateStatus(model: AppointmentRequestsModel): Observable<any> {
    return this.apiService.post<any>('AppointmentRequests/DoctorAppointmentUpdateStatus', model);
  }

  GetPatientDetails(aadhaarNumber: string): Observable<AppointmentRequestsModel> {
    return this.apiService.get<AppointmentRequestsModel>('AppointmentRequests/GetPatientDetails', {
      aadhaarNumber: aadhaarNumber
    });
  }
    

  SavePatientAppointment(formData: FormData): Observable<any> {
    return this.apiService.post<any>('AppointmentRequests/SavePatientAppointment', formData);
  }
    
  RescheduleAppointment(model: AppointmentRequestsModel): Observable<any> {
    return this.apiService.post<any>('AppointmentRequests/DoctorAppointmentUpdateStatus', model);
  }
    
}
