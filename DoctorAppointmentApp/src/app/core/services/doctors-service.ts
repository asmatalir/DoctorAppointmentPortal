import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DoctorsModel } from '../models/DoctorsModel';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DoctorsService {
  constructor(private http: HttpClient) { }

  DoctorsGetList(model: DoctorsModel): Observable<DoctorsModel[]> {
    return this.http.post<DoctorsModel[]>(environment.apiBaseUrl + 'Doctors/GetLists',model);
  }

  GetDoctorDetails(id: number): Observable<any> {
    return this.http.get<any>(environment.apiBaseUrl + 'Doctors/GetDoctorDetails', { params: { id: id.toString() } }
    );
  }
  
  SaveDoctorDetails(doctor: DoctorsModel): Observable<any> {
    return this.http.post<any>(environment.apiBaseUrl + 'Doctors/SaveAddEditDoctor', doctor);
  }
  
  saveDoctorAvailability(doctor: DoctorsModel): Observable<any> { 
    return this.http.post<any>(environment.apiBaseUrl + 'Doctors/SaveDoctorAvailability', doctor );
  }
  
  
}
