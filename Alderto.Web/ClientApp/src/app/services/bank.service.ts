import { Injectable } from '@angular/core';
import { IGuildBank, IGuildBankItem } from 'src/app/models';
import { AldertoWebBankApi } from './web';
import { Guild } from '.';

export class GuildBank {
    public readonly id: number;
    public readonly guildId: string;

    public name: string;

    public logChannelId: string;
    public moderatorRoleId: string;

    public contents: IGuildBankItem[];

    public value: number;

    public userCanModify: boolean;

    constructor(bank: IGuildBank, private guild: Guild) {
        this.id = bank.id;
        this.guildId = bank.guildId;

        this.name = bank.name;

        this.logChannelId = bank.logChannelId;
        this.moderatorRoleId = bank.moderatorRoleId;

        this.contents = bank.contents;

        if (this.contents != null)
            this.updateValue();

        this.updateCanModify();
    }

    public updateValue() {
        this.value = this.contents.reduce((acc, current) => acc + current.quantity * current.value, 0);
        return this.value;
    }

    private async updateCanModify() {
        // If user is admin as is
        if (this.guild.userIsAdmin)
            this.userCanModify = true;

        // If mod role is not defined
        else if (this.moderatorRoleId == null)
            this.userCanModify = false;

        // Check if user has said role
        else
            this.userCanModify = (await this.guild.userRoles).find(o => o.id === this.moderatorRoleId) !== undefined;

        return this.userCanModify;
    }
}

@Injectable({
    providedIn: 'root'
})
export class BankService {
    private bankMap = {};

    constructor(
        public readonly bankApi: AldertoWebBankApi
    ) { }

    public async getBanks(guild: Guild): Promise<GuildBank[]> {
        const bank = this.bankMap[guild.id];
        if (bank !== undefined)
            return bank;

        this.bankMap[guild.id] = [];
        (await this.bankApi.fetchBanks(guild.id).toPromise()).forEach(b => this.bankMap[guild.id].push(new GuildBank(b, guild)));
        return this.bankMap[guild.id];
    }
}
