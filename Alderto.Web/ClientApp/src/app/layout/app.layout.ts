import { Component } from '@angular/core';
import { NavigationService } from '../services';

@Component({
  selector: 'body',
  templateUrl: 'app.layout.html'
})
export class AppComponent {
  constructor(public readonly nav: NavigationService) { }
}
