import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NavigationProviderService } from '../../services';

@Component({
  template: '<router-outlet></router-outlet>'
})
export class GuildLayoutComponent implements OnInit {
  public constructor(
    private readonly nav: NavigationProviderService,
    private readonly route: ActivatedRoute) { }

  public ngOnInit() {
    this.nav.navigateToGuild(this.route.snapshot.params['id'] as number);
  }
}
