import { Component, ViewChild } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';

import { FormBuilder, Validators } from '@angular/forms';

import { GuildBankService } from 'src/app/services/web-api';

@Component({
  selector: 'app-bank-remove',
  templateUrl: 'bank-remove.component.html'
})
export class BankRemoveComponent {
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
  }
}
