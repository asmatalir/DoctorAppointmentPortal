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
  
}
