import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BooksModel } from '../../Models/BooksModel';
import { environment } from '../environment/environment';

@Injectable({
  providedIn: 'root'
})
export class AddEditService {


  constructor(private http: HttpClient) {}

  AddEdit(id : number): Observable<any> {
    debugger;
    return this.http.get<any>(environment.apiBaseUrl + 'Books/AddEdit' , { params: { id: id.toString() } });
  }

  SaveAddEdit(model : BooksModel) : Observable<any> {
    return this.http.post<any>(environment.apiBaseUrl + 'Books/SaveAddEdit', model);
  }
}

