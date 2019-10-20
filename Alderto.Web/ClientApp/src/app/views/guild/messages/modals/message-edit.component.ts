import { Component, OnInit, OnDestroy } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

import { GuildService, AldertoWebMessageApi, User } from 'src/app/services';
import { IGuildRole, IManagedMessage } from 'src/app/models';
import { HttpErrorResponse } from '@angular/common/http';
import { Subscription, Subject } from 'rxjs';

@Component({
    templateUrl: 'message-edit.component.html'
})
export class MessageEditComponent implements OnInit, OnDestroy {
    // Input
    public message: IManagedMessage;

    // Output
    public onMessageEdited: Subject<void>;

    public roleSelect: IGuildRole[];

    public formGroup: FormGroup;
    public userIsAdmin: boolean;

    private subscriptions: Subscription[] = [];

    constructor(
        private readonly fb: FormBuilder,
        private readonly messageApi: AldertoWebMessageApi,
        private readonly guild: GuildService,
        private readonly toastr: ToastrService,
        public readonly modal: BsModalRef) {
    }

    public async ngOnInit() {
        this.onMessageEdited = new Subject();

        this.formGroup = this.fb.group({
            moderatorRoleId: [this.message.moderatorRoleId],
            content: [this.message.content, Validators.required]
        });

        this.roleSelect = await this.guild.currentGuild.roles;
        this.userIsAdmin = this.guild.currentGuild.userIsAdmin;
    }
    public ngOnDestroy() {
        this.onMessageEdited.complete();
        this.subscriptions.forEach(s => s.unsubscribe());
    }

    public onSubmit() {
        if (!this.formGroup.valid)
            return;

        this.messageApi.editMessage(
            this.guild.currentGuildId,
            this.message.id,
            {
                content: this.formGroup.value.content,
                moderatorRoleId: this.userIsAdmin
                    ? this.formGroup.value.moderatorRoleId != null
                        ? this.formGroup.value.moderatorRoleId
                        : "0"
                    : null
            } as IManagedMessage
        ).subscribe(
            () => {
                this.message.content = this.formGroup.value.content;
                this.message.moderatorRoleId = this.formGroup.value.moderatorRoleId;
                this.message.lastModified = new Date();

                // Emit the created message.
                this.onMessageEdited.next();

                // Display success toast.
                this.toastr.success(`Successfully edited the message.`);
            },
            (err: HttpErrorResponse) => {
                this.toastr.error(err.error.message, 'Could not edit the message');
            },
            () => {
                this.modal.hide();
            });
    }
}
