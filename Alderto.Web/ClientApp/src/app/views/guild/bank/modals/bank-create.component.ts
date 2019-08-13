import { Component, ViewChild } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';

import { FormBuilder, Validators } from '@angular/forms';

import { IGuildBank } from '../../../../models';
import { GuildBankService } from '../../../../services/web-api';

@Component({
  selector: 'app-bank-create',
  templateUrl: 'bank-create.component.html'
})
export class BankCreateComponent {
  @ViewChild('modal', { static: false }) public modal: ModalDirective;

  public formGroup = this.fb.group({
    name: ['', Validators.required]
  });

  constructor(
    private readonly fb: FormBuilder,
    private readonly bank: GuildBankService) { }

  public show() {
    this.modal.show();
  }

  public onSubmit() {
    console.log(this.formGroup.value);
    this.bank.createNewBank(this.formGroup.value.name).subscribe(r => { this.modal.hide(); });
  }
}
