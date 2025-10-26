import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DoctorsModel } from '../models/DoctorsModel';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LocationService {
    constructor(private http: HttpClient) {}

  getStates(): Observable<any[]> {
    return this.http.get<any>(environment.apiBaseUrl + 'Location/GetStates');
  }

  getDistricts(stateId: number): Observable<any[]> {
    return this.http.get<any>(environment.apiBaseUrl + 'Location/GetDistricts', { params: { stateId: stateId.toString() } });
  }

  getTalukas(districtId: number): Observable<any[]> {
    return this.http.get<any>(environment.apiBaseUrl + 'Location/GetTalukas', { params: { districtId: districtId.toString() } });
  }

  getCities(talukaId: number): Observable<any[]> {
    return this.http.get<any>(environment.apiBaseUrl + 'Location/GetCities', { params: { talukaId: talukaId.toString() } });
  }
}
