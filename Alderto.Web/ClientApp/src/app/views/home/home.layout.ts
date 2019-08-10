import { Component, OnInit } from '@angular/core';
import { NavigationProviderService } from '../../services';

@Component({
  template: '<router-outlet></router-outlet>'
})
export class HomeLayoutComponent implements OnInit {
  public constructor(
    private readonly nav: NavigationProviderService) { }

  public ngOnInit() {
    this.nav.navigateToHome();
  }
}
