import { Component, OnInit } from '@angular/core';
import { IGuildBank } from 'src/app/models';
import { GuildBankService } from 'src/app/services';

@Component({
  templateUrl: 'overview.component.html'
})
export class OverviewComponent implements OnInit {
  public guildBanks: IGuildBank[] = [];

  constructor(private readonly gb: GuildBankService) { }

  public ngOnInit(): void {
    this.gb.fetchBanks().subscribe(b => this.guildBanks = b);
  }
}
