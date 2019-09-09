import { Component, OnInit, OnDestroy } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

import { AldertoWebBankApi } from 'src/app/services';
import { IGuildBank } from 'src/app/models';
import { HttpErrorResponse } from '@angular/common/http';
import { IGuildBankItem } from 'src/app/models/guild-bank';
import { Subject } from 'rxjs';

@Component({
  templateUrl: 'bank-items-create.component.html'
})
export class BankItemsCreateComponent implements OnInit, OnDestroy {
  public bank: IGuildBank;

  public onItemAdded: Subject<IGuildBankItem>;

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
    private readonly toastr: ToastrService,
    public readonly modal: BsModalRef) {
  }

  public ngOnInit() {
    this.onItemAdded = new Subject();
  }

  public ngOnDestroy() {
    this.onItemAdded.complete();
  }

  public onSubmit() {
    if (!this.formGroup.valid)
      return;

    this.bankApi.createNewBankItem(this.bank.guildId, this.bank.id,
      {
        name: this.formGroup.value.name,
        description: this.formGroup.value.description,
        value: this.formGroup.value.value,
        quantity: this.formGroup.value.quantity,
        imageUrl: this.formGroup.value.imageUrl
      } as IGuildBankItem).subscribe(
        item => {
          this.onItemAdded.next(item);
          this.toastr.success(`Successfully added a new item <b>${item.name}</b>`, null, { enableHtml: true });
        },
        (err: HttpErrorResponse) => {
          this.toastr.error(err.error.message, 'Could not add the item');
        },
        () => {
          this.modal.hide();
        });
  }
}
