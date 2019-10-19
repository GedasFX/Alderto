import { Component, OnInit, OnDestroy } from '@angular/core';
import { IManagedMessage } from 'src/app/models';
import { AldertoWebMessageApi, GuildService } from 'src/app/services';
import { BsModalService } from 'ngx-bootstrap/modal';
import { Subject, Subscription } from 'rxjs';

import { MessageCreateComponent } from './modals/message-create.component';
import { MessageRemoveComponent } from './modals/message-remove.component';
import { MessageEditComponent } from './modals/message-edit.component';

@Component({
    templateUrl: 'overview.component.html',
    styleUrls: ['overview.component.scss']
})
export class OverviewComponent implements OnInit, OnDestroy {
    public messages: IManagedMessage[];

    public userIsAdmin: boolean = false;

    private subscriptions: Subscription[] = [];

    constructor(
        private readonly messageApi: AldertoWebMessageApi,
        private readonly guild: GuildService,
        private readonly modal: BsModalService) {
    }

    public ngOnInit(): void {
        this.subscriptions.push(
            this.guild.currentGuild$.subscribe(g => {
                if (g !== undefined) {
                    this.userIsAdmin = g.userIsAdmin;

                    this.messageApi.fetchMessages(g.id).subscribe(messages => {
                        this.messages = messages;
                    });
                }
            }));
    }

    public ngOnDestroy(): void {
        this.subscriptions.forEach(s => s.unsubscribe());
    }

    public openMessageCreateModal(): void {
        const modal = this.modal.show(MessageCreateComponent,
            {
                ignoreBackdropClick: true
            });
        (modal.content.onMessageCreated as Subject<IManagedMessage>).subscribe(b => {
            this.messages.push(b);
        });
    }

    public openMessageEditModal(message: IManagedMessage): void {
        this.modal.show(MessageEditComponent,
            {
                initialState: { message },
                ignoreBackdropClick: true
            });
    }

    public openMessageRemoveModal(message: IManagedMessage): void {
        const modal = this.modal.show(MessageRemoveComponent,
            {
                initialState: { messages: this.messages, message },
                ignoreBackdropClick: true
            });
        (modal.content.onMessageDeleted as Subject<void>).subscribe(() => {
            this.messages.splice(this.messages.indexOf(message), 1);
        });
    }

    private groupBy(xs, key) {
        return xs.reduce((rv, x) => {
            (rv[x[key]] = rv[x[key]] || []).push(x);
            return rv;
        }, {});
    }
}
