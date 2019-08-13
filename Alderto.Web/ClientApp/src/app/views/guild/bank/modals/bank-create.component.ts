import { Component, ViewChild, Input } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FormBuilder, Validators } from '@angular/forms';

import { GuildBankService } from 'src/app/services';
import { IGuildBank } from 'src/app/models';

@Component({
  selector: 'app-bank-create',
  templateUrl: 'bank-create.component.html'
})
export class BankCreateComponent {
  @ViewChild('modal', { static: false })
  public modal: ModalDirective;

  @Input()
  public banks: IGuildBank[];

  public formGroup = this.fb.group({
    name: ['', Validators.required]
  });

  constructor(
    private readonly fb: FormBuilder,
    private readonly bank: GuildBankService) {
  }

  public show() {
    this.modal.show();
  }

  public onSubmit() {
    this.bank.createNewBank(this.formGroup.value.name).subscribe(r => {
      this.banks.push(r);
      this.modal.hide();
    });
  }
}
