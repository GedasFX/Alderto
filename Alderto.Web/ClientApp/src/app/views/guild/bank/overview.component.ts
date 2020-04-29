import { Component, OnInit, OnDestroy } from '@angular/core';
import { IGuildBankItem, IGuildBank } from 'src/app/models';
import { GuildService, Guild, GuildBank, BankService } from 'src/app/services';
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
  private currentGuild: Guild;
  public guildBanks: GuildBank[] = [];

  public userIsAdmin: boolean;

  private subscriptions: Subscription[] = [];

  constructor(
    private readonly bankService: BankService,
    private readonly guild: GuildService,
    private readonly modal: BsModalService) {
  }

  public ngOnInit(): void {
    this.subscriptions.push(
      this.guild.currentGuild$.subscribe(async g => {
        if (g) {
          this.currentGuild = g;
          this.userIsAdmin = g.userIsAdmin;
          this.guildBanks = await this.bankService.getBanks(g);
        }
      })
    );
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
      this.guildBanks.push(new GuildBank(b, this.currentGuild));
    });
  }

  public openBankEditModal(bank: GuildBank): void {
    this.modal.show(BankEditComponent,
      {
        initialState: { bank },
        ignoreBackdropClick: true
      });
  }

  public openBankRemoveModal(bank: GuildBank): void {
    const modal = this.modal.show(BankRemoveComponent,
      {
        initialState: { bank },
        ignoreBackdropClick: true
      });
    (modal.content.onBankDeleted as Subject<void>).subscribe(() => {
      this.guildBanks.splice(this.guildBanks.indexOf(bank), 1);
    });
  }


  public openItemCreateModal(bank: GuildBank): void {
    const modal = this.modal.show(BankItemsCreateComponent,
      {
        initialState: { bank },
        ignoreBackdropClick: true
      });

    (modal.content.onItemAdded as Subject<IGuildBankItem>).subscribe(item => {
      bank.contents.push(item);
      bank.updateValue();
    });
  }

  public openItemDetailsModal(bank: GuildBank, item: IGuildBankItem) {
    const modal = this.modal.show(BankItemsDetailsComponent,
      {
        initialState: { item, canModify: bank.userCanModify },
        ignoreBackdropClick: true
      });
    (modal.content.onItemEdited as Subject<void>).subscribe(() => {
      bank.updateValue();
    });
    (modal.content.onItemDeleted as Subject<void>).subscribe(() => {
      bank.contents.splice(bank.contents.indexOf(item), 1);
    });
  }
}
