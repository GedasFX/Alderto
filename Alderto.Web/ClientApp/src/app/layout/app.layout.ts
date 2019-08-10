import { Component, OnInit } from '@angular/core';
import { NavigationService } from '../services';
import { Observable } from 'rxjs';
import { INavData } from '../_nav';

@Component({
  selector: 'body',
  templateUrl: 'app.layout.html'
})
export class AppComponent {
  constructor(public readonly nav: NavigationService) { }
}
