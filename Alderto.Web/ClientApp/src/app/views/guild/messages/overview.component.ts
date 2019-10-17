import { Component, OnInit, OnDestroy } from '@angular/core';
import { IGuildBank, IGuildBankItem } from 'src/app/models';
import { AldertoWebBankApi, NavigationService, GuildService } from 'src/app/services';
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
    public guildBanks: IGuildBank[] = [];
    public bankValues = {};

    public isAdmin: boolean;

    private subscriptions: Subscription[] = [];

    constructor(
        private readonly bankApi: AldertoWebBankApi,
        private readonly nav: NavigationService,
        private readonly guild: GuildService,
        private readonly modal: BsModalService) {
    }

    public updateValue(bank: IGuildBank): number {
        if (bank.contents == null) {
            this.bankValues[bank.id] = 0;
            return 0;
        }

        this.bankValues[bank.id] = bank.contents.reduce((acc, current) => acc + current.quantity * current.value, 0);
        return this.bankValues[bank.id];
    }


    public ngOnInit(): void {
        this.subscriptions.push(this.guild.currentGuild$.subscribe(g => {
            if (g !== undefined)
                this.isAdmin = g.isAdmin;
        }));
        this.bankApi.fetchBanks(this.nav.getCurrentGuildId()).subscribe(banks => {
            this.guildBanks = banks;
            this.guildBanks.forEach(b => this.updateValue(b));
        });
    }

    public ngOnDestroy(): void {
        this.subscriptions.forEach(s => s.unsubscribe());
    }

    public openMessageCreateModal(): void {
        const modal = this.modal.show(MessageCreateComponent,
            {
                ignoreBackdropClick: true
            });
        (modal.content.onBankCreated as Subject<IGuildBank>).subscribe(b => {
            this.guildBanks.push(b);
            this.bankValues[b.id] = this.updateValue(b);
        });
    }

    public openMessageEditModal(bank: IGuildBank): void {
        this.modal.show(MessageEditComponent,
            {
                initialState: { bank },
                ignoreBackdropClick: true
            });
    }

    public openMessageRemoveModal(bank: IGuildBank): void {
        const modal = this.modal.show(MessageRemoveComponent,
            {
                initialState: { banks: this.guildBanks, bank },
                ignoreBackdropClick: true
            });
        (modal.content.onBankDeleted as Subject<void>).subscribe(() => {
            this.guildBanks.splice(this.guildBanks.indexOf(bank), 1);
        });
    }
}
