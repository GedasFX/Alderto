import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

import { AldertoWebBankApi } from 'src/app/services';
import { IGuildBank } from 'src/app/models';
import { HttpErrorResponse } from '@angular/common/http';
import { IGuildBankItem } from 'src/app/models/guild-bank';

@Component({
  templateUrl: 'bank-items-create.component.html'
})
export class BankItemsCreateComponent implements OnInit {
  public bank: IGuildBank;

  public formGroup = this.fb.group({
    name: [null, [Validators.required, Validators.maxLength(70)]],
    description: [null, Validators.maxLength(280)],
    value: [null, [Validators.required, Validators.min(-1000000000000000000), Validators.max(1000000000000000000)]],
    quantity: [null, [Validators.required, Validators.min(-1000000000000000000), Validators.max(1000000000000000000)]],
    imageUrl: [null, [Validators.maxLength(140)]]
  });

  constructor(
    private readonly fb: FormBuilder,
    private readonly bankApi: AldertoWebBankApi,
    private readonly modal: BsModalRef,
    private readonly toastr: ToastrService) {
  }

  public ngOnInit() {
  }

  public onSubmit() {
    if (!this.formGroup.valid)
      return;

    console.log(this);

    this.bankApi.createNewBankItem(this.bank.guildId, this.bank.id,
      {
        name: this.formGroup.value.name,
        description: this.formGroup.value.description,
        value: this.formGroup.value.value,
        quantity: this.formGroup.value.quantity,
        imageUrl: this.formGroup.value.imageUrl
      } as IGuildBankItem).subscribe(
        item => {
          this.bank.contents.push(item);
          this.toastr.success(`Successfully created bank item <b>${item.name}</b>`, null, { enableHtml: true });
        },
        (err: HttpErrorResponse) => {
          this.toastr.error(err.error.message, 'Could not create the item');
        },
        () => {
          this.modal.hide();
        });
  }
}
