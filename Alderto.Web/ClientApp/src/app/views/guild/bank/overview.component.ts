import { Component, OnInit, ViewChild } from '@angular/core';
import { IGuildBank } from '../../../models/guild-bank';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GuildBankService } from '../../../services/web-api';

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
