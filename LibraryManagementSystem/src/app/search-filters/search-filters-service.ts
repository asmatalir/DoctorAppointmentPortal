import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BooksModel } from '../../Models/BooksModel';
import { environment } from '../environment/environment';

@Injectable({
  providedIn: 'root'
})
export class SearchFiltersService {


  constructor(private http: HttpClient) {}

  GetLists(): Observable<any> {
    debugger;
    return this.http.get<any>(environment.apiBaseUrl + 'Books/GetLists');
  }
}
