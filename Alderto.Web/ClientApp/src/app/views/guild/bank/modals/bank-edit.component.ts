import { Component, OnInit, OnDestroy } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

import { AldertoWebBankApi, GuildService } from 'src/app/services';
import { IGuildBank, IGuildChannel, IGuildRole } from 'src/app/models';
import { HttpErrorResponse } from '@angular/common/http';
import { Subscription } from 'rxjs';

@Component({
    templateUrl: 'bank-edit.component.html'
})
export class BankEditComponent implements OnInit, OnDestroy {
    // Input
    public bank: IGuildBank;

    public channelSelect: IGuildChannel[];
    public roleSelect: IGuildRole[];

    private subscriptions: Subscription[] = [];

    public formGroup: FormGroup;

    constructor(
        private readonly fb: FormBuilder,
        private readonly bankApi: AldertoWebBankApi,
        private readonly guild: GuildService,
        private readonly toastr: ToastrService,
        public readonly modal: BsModalRef) {
    }

    public ngOnInit() {
        this.formGroup = this.fb.group({
            name: [this.bank.name, Validators.required],
            logChannelId: [this.bank.logChannelId],
            moderatorRoleId: [this.bank.moderatorRoleId]
        });

        this.subscriptions.push(this.guild.currentGuild$.subscribe(g => {
            g.channels.then(c => this.channelSelect = c);
            g.roles.then(r => this.roleSelect = r);
        }));
    }

    public ngOnDestroy() {
        this.subscriptions.forEach(s => s.unsubscribe());
    }

    public onSubmit() {
        if (!this.formGroup.valid)
            return;

        this.bankApi.editBank(this.bank.guildId, {
            id: this.bank.id,
            name: this.formGroup.value.name,
            logChannelId: this.formGroup.value.logChannelId,
            moderatorRoleId: this.formGroup.value.moderatorRoleId
        } as IGuildBank).subscribe(
            () => {
                this.bank.name = this.formGroup.value.name;
                this.bank.logChannelId = this.formGroup.value.logChannelId;
                this.bank.moderatorRoleId = this.formGroup.value.moderatorRoleId;
                console.log(this.formGroup.value);
                this.toastr.success(`Successfully edited bank <b>${this.bank.name}</b>`, null, { enableHtml: true });
            },
            (err: HttpErrorResponse) => {
                this.toastr.error(err.error.message, 'Could not edit the bank');
            },
            () => {
                this.modal.hide();
            });
    }

    public get name() { return this.formGroup.get('name'); }
}
