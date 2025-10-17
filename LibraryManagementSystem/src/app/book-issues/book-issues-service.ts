import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BookIssueModel } from '../../Models/BookIssuesModel';
import { environment } from '../environment/environment';

@Injectable({
  providedIn: 'root'
})
export class BookIssuesService {
  
  constructor(private http: HttpClient) {}

  AddEditIssueBook(id : number): Observable<any> {
    debugger;
    return this.http.get<any>(environment.apiBaseUrl + 'BookIssue/AddEditIssueBook' , { params: { id: id.toString() } });
  }

  SaveAddEditIssueBook(formData : FormData) : Observable<any> {
    debugger;
    return this.http.post<any>(environment.apiBaseUrl + 'BookIssue/SaveBookIssue', formData);
  }

  DownloadFile(storedName: string): Observable<Blob> {
    return this.http.get(environment.apiBaseUrl + 'BookIssue/DownloadFile', {params: { storedName: storedName }, responseType: 'blob' });
  }
  
  
}
