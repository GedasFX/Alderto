import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

import { AldertoWebBankApi, GuildService } from 'src/app/services';
import { IGuildBank, IGuildChannel } from 'src/app/models';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  templateUrl: 'bank-edit.component.html'
})
export class BankEditComponent implements OnInit {
  // List of channels in dropdown.
  public channelSelect: IGuildChannel[];

  public bank: IGuildBank;

  public formGroup: FormGroup;
  
  constructor(
    private readonly fb: FormBuilder,
    private readonly bankApi: AldertoWebBankApi,
    private readonly guild: GuildService,
    private readonly modal: BsModalRef,
    private readonly toastr: ToastrService) {
  }

  public ngOnInit() {
    this.formGroup = this.fb.group({
      name: [this.bank.name, Validators.required],
      logChannelId: [this.bank.logChannelId]
    });

    this.guild.currentGuild$.subscribe(g => {
      if (g !== undefined)
        if (g.channels !== undefined)
          this.channelSelect = g.channels;
        else
          this.guild.updateChannels(g.id)
            .then(channels => this.channelSelect = channels);
    });
  }

  public onSubmit() {
    this.bankApi.editBank(this.bank.guildId, this.bank.id, this.formGroup.value.name, this.formGroup.value.logChannelId).subscribe(
      () => {
        this.bank.name = this.formGroup.value.name;
        this.bank.logChannelId = this.formGroup.value.logChannelId;
        this.toastr.success(`Successfully edited bank <b>${this.bank.name}</b>`, null, { enableHtml: true });
      },
      (err: HttpErrorResponse) => {
        this.toastr.error(err.error.message, 'Could not edit the bank');
      },
      () => {
        this.modal.hide();
      });
  }
}
