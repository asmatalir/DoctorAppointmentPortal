import { Component,ChangeDetectorRef } from '@angular/core';
import { Authservice } from '../interceptor/authservice';
import { LanguageService } from '../services/language-service';


@Component({
  selector: 'app-navbar',
  standalone: false,
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class Navbar {
  username: string | null = null;
  constructor( private cdRef: ChangeDetectorRef,private authservice: Authservice,public lang: LanguageService) {}
  getUserName() : string | null {
    return this.authservice.getUsername();
  }
  
  logout(){
    this.authservice.logout();
  }
}
