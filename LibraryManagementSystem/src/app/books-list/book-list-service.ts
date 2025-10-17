import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BooksModel } from '../../Models/BooksModel';
import { environment } from '../environment/environment';

@Injectable({
  providedIn: 'root'
})
export class BookListService {


  constructor(private http: HttpClient) {}

  Bookgetlist(model: BooksModel): Observable<any> {
    return this.http.post<any>(environment.apiBaseUrl + 'Books/Bookgetlist', model);
  }


  BookDelete(bookId : number): Observable<any> {
    debugger;
    return this.http.post<any>(environment.apiBaseUrl + `Books/BookDelete?BookId=${bookId}`,{});
  }
}
