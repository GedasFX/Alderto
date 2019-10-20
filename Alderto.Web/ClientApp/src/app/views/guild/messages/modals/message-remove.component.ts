import { Component, OnInit, OnDestroy } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { GuildService, AldertoWebMessageApi } from 'src/app/services';
import { IManagedMessage } from 'src/app/models';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';


@Component({
    templateUrl: 'message-remove.component.html'
})
export class MessageRemoveComponent implements OnInit, OnDestroy {
    // Input
    public message: IManagedMessage;

    public onMessageDeleted: Subject<void>;

    constructor(
        private readonly messageApi: AldertoWebMessageApi,
        private readonly guild: GuildService,
        private readonly toastr: ToastrService,
        public readonly modal: BsModalRef) {
    }

    public ngOnInit(): void {
        this.onMessageDeleted = new Subject();
    }

    public ngOnDestroy(): void {
        this.onMessageDeleted.complete();
    }

    public onDeleteConfirmed() {
        this.messageApi.removeMessage(this.guild.currentGuildId, this.message.id).subscribe(() => {
            this.onMessageDeleted.next();
            this.toastr.success('Successfully removed the message');
        },
            (err: HttpErrorResponse) => {
                this.toastr.error(err.error.message, 'Could not remove the bank');
            },
            () => {
                this.modal.hide();
            });
    }
}
