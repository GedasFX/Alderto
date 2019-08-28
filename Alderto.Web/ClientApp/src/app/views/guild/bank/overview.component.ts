import { Component, OnInit } from '@angular/core';
import { IGuildBank } from 'src/app/models';
import { AldertoWebBankApi, NavigationService } from 'src/app/services';

@Component({
  templateUrl: 'overview.component.html'
})
export class OverviewComponent implements OnInit {
  public guildBanks: IGuildBank[] = [];

  constructor(
    private readonly bankApi: AldertoWebBankApi,
    private readonly nav: NavigationService) { }

  public ngOnInit(): void {
    this.bankApi.fetchBanks(this.nav.getCurrentGuildId()).subscribe(b => this.guildBanks = b);
  }
}
