import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class LocationService {
    constructor(private apiService : ApiService) {}

  getStates(): Observable<any[]> {
    return this.apiService.get<any[]>('Location/GetStates');
  }

  getDistricts(stateId: number): Observable<any[]> {
    return this.apiService.get<any[]>('Location/GetDistricts', { stateId: stateId.toString() });
  }

  getTalukas(districtId: number): Observable<any[]> {
    return this.apiService.get<any[]>('Location/GetTalukas', { districtId: districtId.toString() });
  }

  getCities(talukaId: number): Observable<any[]> {
    return this.apiService.get<any[]>('Location/GetCities', { talukaId: talukaId.toString() });
  }
}
