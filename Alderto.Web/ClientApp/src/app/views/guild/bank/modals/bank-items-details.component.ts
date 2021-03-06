import { Component, OnInit, OnDestroy } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

import { AldertoWebBankApi, GuildService } from 'src/app/services';
import { HttpErrorResponse } from '@angular/common/http';
import { IGuildBankItem } from 'src/app/models';
import { Subject } from 'rxjs';

@Component({
  templateUrl: 'bank-items-details.component.html'
})
export class BankItemsDetailsComponent implements OnInit, OnDestroy {
  public item: IGuildBankItem;
  public canModify: any;

  public onItemEdited: Subject<void>;
  public onItemDeleted: Subject<void>;

  public formGroup: FormGroup;

  constructor(
    private readonly fb: FormBuilder,
    private readonly bankApi: AldertoWebBankApi,
    private readonly guild: GuildService,
    private readonly toastr: ToastrService,
    public readonly modal: BsModalRef) {
  }

  public ngOnInit() {
    this.formGroup = this.fb.group({
      name: [{ value: this.item.name, disabled: !this.canModify }, [Validators.required, Validators.maxLength(70)]],
      description: [{ value: this.item.description, disabled: !this.canModify }, Validators.maxLength(280)],
      value: [{ value: this.item.value, disabled: !this.canModify }, [Validators.required, Validators.min(-1000000000000000000), Validators.max(1000000000000000000)]],
      quantity: [{ value: this.item.quantity, disabled: !this.canModify }, [Validators.required, Validators.min(-1000000000000000000), Validators.max(1000000000000000000)]],
      imageUrl: [this.item.imageUrl, [Validators.maxLength(140)]]
    });

    this.onItemEdited = new Subject();
    this.onItemDeleted = new Subject();

    console.log(5);
  }

  public ngOnDestroy() {
    this.onItemEdited.complete();
    this.onItemDeleted.complete();
  }

  public onClickDelete() {
    if (confirm('Are you sure you wish to remove this item?')) {
      this.bankApi.removeBankItem(this.guild.currentGuildId, this.item.guildBankId, this.item.id).subscribe(() => {
        this.onItemDeleted.next();
        this.toastr.success(`Successfully removed item <b>${this.item.name}</b>`, null, { enableHtml: true });
      },
        (err: HttpErrorResponse) => {
          this.toastr.error(err.error.message, 'Could not remove the item');
        },
        () => {
          this.modal.hide();
        });
    }
  }

  public onSubmit() {
    if (!this.formGroup.valid)
      return;
    this.bankApi.editBankItem(this.guild.currentGuildId, this.item.guildBankId, this.item.id,
      {
        name: this.formGroup.value.name,
        description: this.formGroup.value.description,
        value: this.formGroup.value.value,
        quantity: this.formGroup.value.quantity,
        imageUrl: this.formGroup.value.imageUrl
      } as IGuildBankItem).subscribe(
        () => {
          this.item.name = this.formGroup.value.name;
          this.item.description = this.formGroup.value.description;
          this.item.value = this.formGroup.value.value;
          this.item.quantity = this.formGroup.value.quantity;
          this.item.imageUrl = this.formGroup.value.imageUrl;

          this.onItemEdited.next();
          this.toastr.success(`Successfully edited the item <b>${this.item.name}</b>`, null, { enableHtml: true });
        },
        (err: HttpErrorResponse) => {
          this.toastr.error(err.error.message, 'Could not edit the item');
        },
        () => {
          this.modal.hide();
        });
  }
}
