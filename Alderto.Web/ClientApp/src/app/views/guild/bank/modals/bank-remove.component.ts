import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { FormBuilder, Validators } from '@angular/forms';

import { AldertoWebBankApi, NavigationService, GuildService } from 'src/app/services';
import { IGuildBank, IGuildChannel } from 'src/app/models';

@Component({
  selector: 'app-bank-create',
  templateUrl: 'bank-remove.component.html'
})
export class BankRemoveComponent implements OnInit {
  // Reference for output purposes
  public banks: IGuildBank[];

  public bank: IGuildBank;
  public id: number;

  constructor(
    private readonly bankApi: AldertoWebBankApi,
    private readonly guild: GuildService,
    private readonly modal: BsModalRef) {
  }

  public ngOnInit() {
    this.bank = this.banks.find(b => b.id === this.id);
  }

  public onDeleteConfirmed() {
    this.bankApi.removeBank(this.guild.currentGuild$.getValue().id, this.id).subscribe(r => {
      this.banks.splice(this.banks.indexOf(this.bank), 1);
      this.modal.hide();
    });
  }
}
