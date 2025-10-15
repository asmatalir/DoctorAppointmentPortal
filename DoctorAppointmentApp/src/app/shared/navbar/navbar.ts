import { Component,OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../../core/services/user-service';


@Component({
  selector: 'app-navbar',
  standalone: false,
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss'
})
export class Navbar implements OnInit  {
  username : string | null = null;
  role: string | null = null;
  profileImage: string = '/images/Doctor-Profile-Photo.jpg';

  constructor(private userService: UserService, private router: Router) {}

  ngOnInit(): void {
    this.userService.currentUser$.subscribe(user => {
      debugger;
      if (user) {
        this.username = user.username;
        this.role = user.role;
      } else {
        this.username = null;
        this.role = null;
      }
    });
  }

  logout() {
    this.userService.clearUser();
    sessionStorage.clear();
    this.router.navigate(['/login']);
  }
}
