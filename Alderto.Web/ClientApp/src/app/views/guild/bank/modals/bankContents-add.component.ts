import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

import { AldertoWebBankApi, NavigationService, GuildService } from 'src/app/services';
import { IGuildBank } from 'src/app/models';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  templateUrl: 'bank-create.component.html'
})
export class BankContentsAddComponent implements OnInit {
  public bank: IGuildBank;

  public formGroup = this.fb.group({
    name: [null, Validators.required],
    logChannelId: []
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

  public onSubmit() { }
}
