import { Component, OnInit, OnDestroy } from '@angular/core';
import { IManagedMessage } from 'src/app/models';
import { GuildService, Guild, ManagedMessageService, ManagedMessage } from 'src/app/services';
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
    public messages: ManagedMessage[];
    public currentGuild: Guild;
    public channelMap;

    public userIsAdmin: boolean;

    private subscriptions: Subscription[] = [];

    constructor(
        private readonly messageService: ManagedMessageService,
        private readonly guild: GuildService,
        private readonly modal: BsModalService) {
    }

    public ngOnInit(): void {
        this.subscriptions.push(
            this.guild.currentGuild$.subscribe(async g => {
                if (g !== undefined) {
                    this.userIsAdmin = g.userIsAdmin;
                    this.currentGuild = g;
                    this.channelMap = (await g.channels).reduce((map, obj) => {
                        map[obj.id] = obj.name;
                        return map;
                    }, {});
                    this.messages = await this.messageService.getMessages(g);
                }
            })
        );
    }

    public ngOnDestroy(): void {
        this.subscriptions.forEach(s => s.unsubscribe());
    }

    public openMessageCreateModal(): void {
        const modal = this.modal.show(MessageCreateComponent,
            {
                ignoreBackdropClick: true
            });
        (modal.content.onMessageCreated as Subject<IManagedMessage>).subscribe(m => {
            // Group by is stateless. Simple workaround.
            this.messages = this.messages.concat([new ManagedMessage(m, this.currentGuild)]);
        });
    }

    public openMessageEditModal(message: ManagedMessage): void {
        this.modal.show(MessageEditComponent,
            {
                initialState: { message },
                ignoreBackdropClick: true
            });
    }

    public openMessageRemoveModal(message: ManagedMessage): void {
        const modal = this.modal.show(MessageRemoveComponent,
            {
                initialState: { message },
                ignoreBackdropClick: true
            });
        (modal.content.onMessageDeleted as Subject<void>).subscribe(() => {
            this.messages.splice(this.messages.indexOf(message), 1);
        });
    }
}
