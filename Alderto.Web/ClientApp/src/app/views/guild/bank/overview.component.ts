import { Component, OnInit } from '@angular/core';
import { IGuildBank } from 'src/app/models';
import { AldertoWebBankApi, NavigationService } from 'src/app/services';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BankCreateComponent } from './modals/bank-create.component';
import { BankRemoveComponent } from './modals/bank-remove.component';
import { BankEditComponent } from './modals/bank-edit.component';
import { BankItemsCreateComponent } from 'src/app/views/guild/bank/modals/bank-items-create.component';

@Component({
  templateUrl: 'overview.component.html',
  styleUrls: ['overview.component.scss']
})
export class OverviewComponent implements OnInit {
  public guildBanks: IGuildBank[] = [];
  public bankValues = {};

  constructor(
    private readonly bankApi: AldertoWebBankApi,
    private readonly nav: NavigationService,
    private readonly modal: BsModalService) {
  }

  public updateValue(bank: IGuildBank) {
    this.bankValues[bank.id] = bank.contents.reduce((acc, current) => acc + current.quantity * current.value, 0);
  }


  public ngOnInit(): void {
    this.bankApi.fetchBanks(this.nav.getCurrentGuildId()).subscribe(banks => {
      this.guildBanks = banks;
      this.guildBanks.forEach(b => this.updateValue(b));
    });
  }

  public openCreateBankModal(): void {
    this.modal.show(BankCreateComponent,
      {
        initialState: { banks: this.guildBanks },
        ignoreBackdropClick: true
      });
  }

  public openEditBankModal(bank: IGuildBank): void {
    this.modal.show(BankEditComponent,
      {
        initialState: { bank },
        ignoreBackdropClick: true
      });
  }

  public openRemoveBankModal(bank: IGuildBank): void {
    this.modal.show(BankRemoveComponent,
      {
        initialState: { banks: this.guildBanks, bank },
        ignoreBackdropClick: true
      });
  }


  public openCreateItemModal(bank: IGuildBank): void {
    this.modal.show(BankItemsCreateComponent,
      {
        initialState: { bank },
        ignoreBackdropClick: true
      });
  }
}
