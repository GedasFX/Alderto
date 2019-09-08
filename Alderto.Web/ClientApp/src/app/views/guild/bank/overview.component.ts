import { Component, OnInit, OnDestroy } from '@angular/core';
import { IGuildBank, IGuildBankItem } from 'src/app/models';
import { AldertoWebBankApi, NavigationService, GuildService } from 'src/app/services';
import { BsModalService } from 'ngx-bootstrap/modal';
import { Subject, Subscription } from 'rxjs';

import { BankCreateComponent } from './modals/bank-create.component';
import { BankRemoveComponent } from './modals/bank-remove.component';
import { BankEditComponent } from './modals/bank-edit.component';
import { BankItemsCreateComponent } from './modals/bank-items-create.component';
import { BankItemsDetailsComponent } from './modals/bank-items-details.component';

@Component({
  templateUrl: 'overview.component.html',
  styleUrls: ['overview.component.scss']
})
export class OverviewComponent implements OnInit, OnDestroy {
  public guildBanks: IGuildBank[] = [];
  public bankValues = {};

  public isAdmin: boolean;

  private subscriptions: Subscription[] = [];

  constructor(
    private readonly bankApi: AldertoWebBankApi,
    private readonly nav: NavigationService,
    private readonly guild: GuildService,
    private readonly modal: BsModalService) {
  }

  public updateValue(bank: IGuildBank): number {
    this.bankValues[bank.id] = bank.contents.reduce((acc, current) => acc + current.quantity * current.value, 0);
    return this.bankValues[bank.id];
  }


  public ngOnInit(): void {
    this.subscriptions.push(this.guild.currentGuild$.subscribe(g => {
      if (g !== undefined)
        this.isAdmin = g.isAdmin;
    }));
    this.bankApi.fetchBanks(this.nav.getCurrentGuildId()).subscribe(banks => {
      this.guildBanks = banks;
      this.guildBanks.forEach(b => this.updateValue(b));
    });
  }

  public ngOnDestroy(): void {
    this.subscriptions.forEach(s => s.unsubscribe());
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

  public openItemDetailsModal(bank: IGuildBank, item: IGuildBankItem): void {
    const modal = this.modal.show(BankItemsDetailsComponent,
      {
        initialState: { item },
        ignoreBackdropClick: true
      });
    (modal.content.onItemDeleted as Subject<void>).subscribe(() => {
      bank.contents.splice(bank.contents.indexOf(item), 1);
    });
  }
}
