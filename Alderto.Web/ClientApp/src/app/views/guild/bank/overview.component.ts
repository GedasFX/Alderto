import { Component, OnInit } from '@angular/core';
import { IGuildBank } from '../../../models/guild-bank';

@Component({
  templateUrl: 'overview.component.html'
})
export class OverviewComponent implements OnInit {
  public guildBanks: IGuildBank[] = [];

  constructor() { }

  public ngOnInit(): void {

  }

  on() {

  }
}
