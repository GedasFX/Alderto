import { Component } from '@angular/core';
import { NavigationService } from '../services';

@Component({
  selector: 'body',
  templateUrl: 'app.layout.html',
  styleUrls: ['app.layout.scss']
})
export class AppComponent {
  constructor(public readonly nav: NavigationService) { }
}
