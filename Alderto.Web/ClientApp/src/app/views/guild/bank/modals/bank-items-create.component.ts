import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

import { AldertoWebBankApi, NavigationService, GuildService } from 'src/app/services';
import { IGuildBank } from 'src/app/models';
import { HttpErrorResponse } from '@angular/common/http';
import { IGuildBankItem } from 'src/app/models/guild-bank';

@Component({
  templateUrl: 'bank-items-create.component.html'
})
export class BankItemsCreateComponent implements OnInit {
  public bank: IGuildBank;

  public formGroup = this.fb.group({
    name: [null, Validators.required, Validators.maxLength(70)],
    description: [null, Validators.maxLength(280)],
    value: [null, Validators.required, Validators.min(0), Validators.max(1000000000000000000)],
    imageUrl: [null, Validators.maxLength(140)]
  });

  constructor(
    private readonly fb: FormBuilder,
    private readonly bankApi: AldertoWebBankApi,
    private readonly guild: GuildService,
    private readonly nav: NavigationService,
    private readonly modal: BsModalRef,
    private readonly toastr: ToastrService) {
  }

  public ngOnInit() {
  }

  public onSubmit() {
    if (!this.formGroup.valid)
      return;

    this.bankApi.createNewBankItem(this.guild.getCurrentGuildId(),
      {
        name: this.formGroup.value.name,
        description: this.formGroup.value.description,
        value: this.formGroup.value.value,
        imageUrl: this.formGroup.value.imageUrl
      } as IGuildBankItem).subscribe(
        item => {
          this.bank.contents.push(item);
          this.toastr.success(`Successfully created bank item <b>${bank.name}</b>`, null, { enableHtml: true });
        },
        (err: HttpErrorResponse) => {
          this.toastr.error(err.error.message, 'Could not create the bank');
        },
        () => {
          this.modal.hide();
        });
  }
}
