import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../../core/services/user-service';


@Component({
  selector: 'app-navbar',
  standalone: false,
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss'
})
export class Navbar implements OnInit {
  username: string | null = null;
  role: string | null = null;
  profileImage: string = '/images/Doctor-Profile-Photo.jpg';

  constructor(private userService: UserService, public router: Router) { }

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

  menus = {
    Admin: [
      { name: 'Home', link: '' },
      { name: 'Appointments', link: '/admin/appointment-requests' },
      { name: 'Manage Doctors', link: '/admin/doctor-list' },
    ],
    Doctor: [
      { name: 'Home', link: '' },
      { name: 'My Appointments', link: '/doctor/appointment-requests' },
    ],
    Patient: [
      { name: 'Home', link: '' },
      { name: 'Find Doctors', link: '/patient/doctor-list' },
      { name: 'Video Consults', link: '/patient/video-consults' },
    ]
  };

  get menuItems() {
    const key = this.role || 'Patient';
    return this.menus[key as keyof typeof this.menus] || [];
  }


  logout() {
    this.userService.clearUser();
    sessionStorage.clear();
    this.router.navigate(['']);
  }


}
