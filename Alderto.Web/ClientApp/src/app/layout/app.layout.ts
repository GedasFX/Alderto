import { Component, OnInit } from '@angular/core';
import { NavigationProviderService } from '../services/navigation-provider.service';
import { Observable } from 'rxjs';
import { INavData } from '../_nav';

@Component({
  selector: 'body',
  templateUrl: 'app.layout.html'
})
export class AppComponent implements OnInit {
  public navItems$: Observable<INavData[]>;

  constructor(public readonly nav: NavigationProviderService) { }

  public ngOnInit() {
    this.navItems$ = this.nav.navItems$;
  }
}
