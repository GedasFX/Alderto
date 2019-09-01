import { Component, OnInit, OnDestroy } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

import { AldertoWebBankApi, GuildService } from 'src/app/services';
import { IGuildBank, IGuildChannel } from 'src/app/models';
import { HttpErrorResponse } from '@angular/common/http';
import { Subscription, Subject } from 'rxjs';

@Component({
  templateUrl: 'bank-create.component.html'
})
export class BankCreateComponent implements OnInit, OnDestroy {
  public onBankCreated: Subject<IGuildBank>;

  // List of channels in dropdown.
  public channelSelect: IGuildChannel[];

  public formGroup = this.fb.group({
    name: [null, Validators.required],
    logChannelId: []
  });

  constructor(
    private readonly fb: FormBuilder,
    private readonly bankApi: AldertoWebBankApi,
    private readonly guild: GuildService,
    private readonly modal: BsModalRef,
    private readonly toastr: ToastrService) {
  }

  private subscriptions: Subscription[] = [];

  public ngOnInit() {
    this.onBankCreated = new Subject();
    this.subscriptions.push(this.guild.currentGuild$.subscribe(g => {
      if (g !== undefined)
        if (g.channels !== undefined)
          this.channelSelect = g.channels;
        else
          this.guild.updateChannels(g.id)
            .then(channels => this.channelSelect = channels);
    }));
  }

  public ngOnDestroy() {
    this.onBankCreated.complete();
    this.subscriptions.forEach(s => s.unsubscribe());
  }

  public onSubmit() {
    if (!this.formGroup.valid)
      return;

    this.bankApi.createNewBank(this.guild.getCurrentGuildId(),
      {
        name: this.formGroup.value.name,
        logChannelId: this.formGroup.value.logChannelId
      } as IGuildBank).subscribe(
        bank => {
          // Ensure contents are not null for future usage.
          if (bank.contents == null)
            bank.contents = [];

          // Emit the created bank.
          this.onBankCreated.next(bank);

          // Display success toast.
          this.toastr.success(`Successfully created bank <b>${bank.name}</b>`, null, { enableHtml: true });
        },
        (err: HttpErrorResponse) => {
          this.toastr.error(err.error.message, 'Could not create the bank');
        },
        () => {
          this.modal.hide();
        });
  }

  public get name() { return this.formGroup.get('name'); }
}
