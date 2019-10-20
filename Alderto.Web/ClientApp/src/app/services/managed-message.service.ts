import { Injectable } from '@angular/core';
import { IManagedMessage } from 'src/app/models';
import { AldertoWebMessageApi } from './web';
import { Guild } from '.';

export class ManagedMessage {
    public readonly id: string;
    public readonly channelId: string;

    public content: string;
    public lastModified: Date;

    public logChannelId: string;
    public moderatorRoleId: string;

    public userCanModify: boolean;

    constructor(message: IManagedMessage, private guild: Guild) {
        this.id = message.id;
        this.channelId = message.channelId;

        this.content = message.content;
        this.lastModified = message.lastModified;
        
        this.moderatorRoleId = message.moderatorRoleId;

        this.updateCanModify();
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
export class ManagedMessageService {
    private messageMap = {};

    constructor(
        public readonly messageApi: AldertoWebMessageApi
    ) { }

    public async getMessages(guild: Guild): Promise<ManagedMessage[]> {
        const bank = this.messageMap[guild.id];
        if (bank !== undefined)
            return bank;

        this.messageMap[guild.id] = [];
        (await this.messageApi.fetchMessages(guild.id).toPromise()).forEach(m => this.messageMap[guild.id].push(new ManagedMessage(m, guild)));
        return this.messageMap[guild.id];
    }
}
