import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AccountService } from '../services';
import { navItems } from '../_nav';

@Component({
  selector: 'body',
  templateUrl: 'app.component.html'
})
export class AppComponent {
  constructor() { }

  public navItems = navItems;
}
