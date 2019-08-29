import { Component, OnInit } from '@angular/core';
import { IGuildBank } from 'src/app/models';
import { AldertoWebBankApi, NavigationService } from 'src/app/services';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BankCreateComponent } from './modals/bank-create.component';

@Component({
  templateUrl: 'overview.component.html'
})
export class OverviewComponent implements OnInit {
  public guildBanks: IGuildBank[] = [];

  constructor(
    private readonly bankApi: AldertoWebBankApi,
    private readonly nav: NavigationService,
    private readonly modal: BsModalService) {
  }

  public ngOnInit(): void {
    this.bankApi.fetchBanks(this.nav.getCurrentGuildId()).subscribe(b => this.guildBanks = b);
  }

  public openCreateBankModal(): void {
    this.modal.show(BankCreateComponent, { initialState: { banks: this.guildBanks } });
  }
}
