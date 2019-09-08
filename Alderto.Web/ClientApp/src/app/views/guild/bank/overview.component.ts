import { Component, OnInit } from '@angular/core';
import { IGuildBank, IGuildBankItem } from 'src/app/models';
import { AldertoWebBankApi, NavigationService } from 'src/app/services';
import { BsModalService } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';

import { BankCreateComponent } from './modals/bank-create.component';
import { BankRemoveComponent } from './modals/bank-remove.component';
import { BankEditComponent } from './modals/bank-edit.component';
import { BankItemsCreateComponent } from './modals/bank-items-create.component';
import { BankItemsDetailsComponent } from './modals/bank-items-details.component';

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

  public updateValue(bank: IGuildBank): number {
    this.bankValues[bank.id] = bank.contents.reduce((acc, current) => acc + current.quantity * current.value, 0);
    return this.bankValues[bank.id];
  }


  public ngOnInit(): void {
    this.bankApi.fetchBanks(this.nav.getCurrentGuildId()).subscribe(banks => {
      this.guildBanks = banks;
      this.guildBanks.forEach(b => this.updateValue(b));
    });
  }

  public openBankCreateModal(): void {
    const modal = this.modal.show(BankCreateComponent,
      {
        ignoreBackdropClick: true
      });
    (modal.content.onBankCreated as Subject<IGuildBank>).subscribe(b => {
      this.guildBanks.push(b);
      this.bankValues[b.id] = this.updateValue(b);
    });
  }

  public openBankEditModal(bank: IGuildBank): void {
    this.modal.show(BankEditComponent,
      {
        initialState: { bank },
        ignoreBackdropClick: true
      });
  }

  public openBankRemoveModal(bank: IGuildBank): void {
    const modal = this.modal.show(BankRemoveComponent,
      {
        initialState: { banks: this.guildBanks, bank },
        ignoreBackdropClick: true
      });
    (modal.content.onBankDeleted as Subject<void>).subscribe(() => {
      this.guildBanks.splice(this.guildBanks.indexOf(bank), 1);
    });
  }


  public openItemCreateModal(bank: IGuildBank): void {
    const modal = this.modal.show(BankItemsCreateComponent,
      {
        initialState: { bank },
        ignoreBackdropClick: true
      });

    (modal.content.onItemAdded as Subject<IGuildBankItem>).subscribe(item => {
      bank.contents.push(item);
      this.updateValue(bank);
    });
  }

  public openItemDetailsModal(item: IGuildBankItem): void {
    const modal = this.modal.show(BankItemsDetailsComponent,
      {
        initialState: { item },
        ignoreBackdropClick: true
      });
  }
}
