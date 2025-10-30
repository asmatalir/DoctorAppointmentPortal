import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DoctorsModel } from '../models/DoctorsModel';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class DoctorsService {
  constructor(private apiService : ApiService
  ) { }

  DoctorsGetList(model: DoctorsModel): Observable<DoctorsModel[]> {
    return this.apiService.post<DoctorsModel[]>('Doctors/GetLists', model);
  }

  GetDoctorDetails(id: number): Observable<any> {
    return this.apiService.get<any>('Doctors/GetDoctorDetails', { id: id.toString() });
  }
  
  GetDoctorAvailabilityDetails(id: number): Observable<any> {
    return this.apiService.get<any>('Doctors/GetDoctorAvailabilityDetails', { id: id.toString() });
  }

  SaveDoctorDetails(doctor: DoctorsModel): Observable<any> {
    return this.apiService.post<any>('Doctors/SaveAddEditDoctor', doctor);
  }
  
  SaveDoctorAvailability(doctor: DoctorsModel): Observable<any> {
    return this.apiService.post<any>('Doctors/SaveDoctorAvailability', doctor);
  }
  
  GetDoctorAvailableSlots(id: number): Observable<any> {
    return this.apiService.get<any>('Doctors/GetDoctorSlots', { id: id.toString() });
  }
  
}
