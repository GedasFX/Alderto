import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { FormBuilder, Validators } from '@angular/forms';

import { AldertoWebBankApi, NavigationService, GuildService } from 'src/app/services';
import { IGuildBank, IGuildChannel } from 'src/app/models';

@Component({
  selector: 'app-bank-create',
  templateUrl: 'bank-create.component.html'
})
export class BankCreateComponent implements OnInit {
  // Reference for output purposes
  public banks: IGuildBank[];

  // List of channels in dropdown.
  public channelSelect: any[];

  public formGroup = this.fb.group({
    name: ['', Validators.required],
    logChannelId: ['']
  });

  constructor(
    private readonly fb: FormBuilder,
    private readonly bankApi: AldertoWebBankApi,
    private readonly guild: GuildService,
    private readonly nav: NavigationService,
    private readonly modal: BsModalRef) {
  }

  public ngOnInit() {
    this.guild.currentGuild$.subscribe(g => {
      if (g.channels !== undefined)
        this.channelSelect = g.channels;
      else
        this.guild.updateChannels(g.id)
          .then(channels => this.channelSelect = channels);
    });
  }

  public onSubmit() {
    this.bankApi.createNewBank(this.nav.getCurrentGuildId(), this.formGroup.value.name, this.formGroup.value.logChannelId).subscribe(r => {
      this.banks.push(r);
      this.modal.hide();
    });
  }
}
