import { Component, OnInit, OnDestroy } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

import { AldertoWebBankApi, GuildService } from 'src/app/services';
import { IGuildBank, IGuildChannel, IGuildRole } from 'src/app/models';
import { HttpErrorResponse } from '@angular/common/http';
import { Subscription } from 'rxjs';

@Component({
  templateUrl: 'message-edit.component.html'
})
export class MessageEditComponent implements OnInit, OnDestroy {
  public ngOnInit(): void { }

  public ngOnDestroy(): void { }
}
