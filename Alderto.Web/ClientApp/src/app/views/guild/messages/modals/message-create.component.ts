import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

import { GuildService, AldertoWebMessageApi } from 'src/app/services';
import { IGuildChannel, IGuildRole, IManagedMessage } from 'src/app/models';
import { HttpErrorResponse } from '@angular/common/http';
import { Subscription, Subject } from 'rxjs';
import { TabsetComponent } from 'ngx-bootstrap/tabs';


@Component({
    templateUrl: 'message-create.component.html'
})
export class MessageCreateComponent implements OnInit, OnDestroy {
    @ViewChild('tabset') tabset: TabsetComponent;

    public onMessageCreated: Subject<IManagedMessage>;

    // List of channels in dropdown.
    public channelSelect: IGuildChannel[];
    public roleSelect: IGuildRole[];

    public importGroup = this.fb.group({
        channelId: [null, Validators.required],
        moderatorRoleId: [null],
        messageId: [null, [Validators.required, Validators.pattern('^[0-9]+$')]],
    });

    public createGroup = this.fb.group({
        channelId: [null, Validators.required],
        moderatorRoleId: [null],
        content: [null, Validators.required]
    });

    private subscriptions: Subscription[] = [];

    constructor(
        private readonly fb: FormBuilder,
        private readonly messageApi: AldertoWebMessageApi,
        private readonly guild: GuildService,
        private readonly toastr: ToastrService,
        public readonly modal: BsModalRef) {
    }

    public async ngOnInit() {
        this.onMessageCreated = new Subject();
        this.channelSelect = await this.guild.currentGuild.channels;
        this.roleSelect = await this.guild.currentGuild.roles;
    }

    public ngOnDestroy() {
        this.onMessageCreated.complete();
        this.subscriptions.forEach(s => s.unsubscribe());
    }

    public onCreate() {
        if (!this.createGroup.valid)
            return;

        this.messageApi.createNewMessage(
            this.guild.currentGuildId,
            this.createGroup.value.channelId,
            this.createGroup.value.content,
            this.createGroup.value.moderatorRoleId,
        ).subscribe(
            message => {
                // Emit the created message.
                this.onMessageCreated.next(message);

                // Display success toast.
                this.toastr.success(`Successfully posted a new message.`);
            },
            (err: HttpErrorResponse) => {
                this.toastr.error(err.error.message, 'Could not post a new message');
            },
            () => {
                this.modal.hide();
            });
    }

    public onImport() {
        if (!this.importGroup.valid)
            return;

        this.messageApi.importMessage(
            this.guild.currentGuildId,
            this.importGroup.value.channelId,
            this.importGroup.value.messageId,
            this.importGroup.value.moderatorRoleId
        ).subscribe(
            message => {
                // Emit the created message.
                this.onMessageCreated.next(message);

                // Display success toast.
                this.toastr.success(`Successfully imported the message.`);
            },
            (err: HttpErrorResponse) => {
                this.toastr.error(err.error.message, 'Could not import the message');
            },
            () => {
                this.modal.hide();
            });
    }

    public onSubmit() {
        // If Post new selected
        if (this.tabset.tabs[0].active) {
            this.onCreate();
        } else { // If import selected
            this.onImport();
        }
    }
}
