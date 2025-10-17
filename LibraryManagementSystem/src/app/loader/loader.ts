import { Component } from '@angular/core';
import { LoaderService } from './loader-service';

@Component({
  selector: 'app-loader',
  standalone: false,
  templateUrl: './loader.html',
  styleUrl: './loader.css'
})
export class Loader {
  constructor(public loaderService: LoaderService) {}
}
