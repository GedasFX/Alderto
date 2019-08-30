import { Component } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { AldertoWebBankApi, GuildService } from 'src/app/services';
import { IGuildBank } from 'src/app/models';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-bank-create',
  templateUrl: 'bank-remove.component.html'
})
export class BankRemoveComponent {
  // Reference for output purposes
  public banks: IGuildBank[];

  public bank: IGuildBank;

  constructor(
    private readonly bankApi: AldertoWebBankApi,
    private readonly guild: GuildService,
    private readonly modal: BsModalRef,
    private readonly toastr: ToastrService) {
  }

  public onDeleteConfirmed() {
    this.bankApi.removeBank(this.guild.currentGuild$.getValue().id, this.bank.id).subscribe(r => {
      this.banks.splice(this.banks.indexOf(this.bank), 1);
      this.toastr.success(`Successfully removed bank <b>${this.bank.name}</b>`, null, { enableHtml: true });
    },
    (err: HttpErrorResponse) => {
      this.toastr.error(err.error.message, 'Could not remove the bank');
    },
    () => {
      this.modal.hide();
    });
  }
}
