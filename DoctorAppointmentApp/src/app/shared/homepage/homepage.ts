import { Component } from '@angular/core';

@Component({
  selector: 'app-homepage',
  standalone: false,
  templateUrl: './homepage.html',
  styleUrl: './homepage.scss'
})
export class Homepage {
ngOnInit(): void {
  history.pushState(null, '');
  window.addEventListener('popstate', () => {
    history.pushState(null, '');
  });
}
}
