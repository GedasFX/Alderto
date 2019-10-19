import { Component, OnInit, OnDestroy } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

import { AldertoWebBankApi, GuildService } from 'src/app/services';
import { IGuildBank, IGuildChannel, IGuildRole } from 'src/app/models';
import { HttpErrorResponse } from '@angular/common/http';
import { Subscription, Subject } from 'rxjs';

@Component({
    templateUrl: 'message-create.component.html'
})
export class MessageCreateComponent implements OnInit, OnDestroy {
    public ngOnInit(): void { }

    public ngOnDestroy(): void { }
}
