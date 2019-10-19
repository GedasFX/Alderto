import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IManagedMessage } from 'src/app/models';

@Injectable({
    providedIn: 'root'
})
export class AldertoWebMessageApi {
    constructor(private readonly http: HttpClient) { }

    public fetchMessages(guildId: string): Observable<IManagedMessage[]> {
        return this.http.get<IManagedMessage[]>(`/api/guilds/${guildId}/messages`);
    }

    public createNewMessage(guildId: string, channelId: string, content: string): Observable<IManagedMessage> {
        return this.http.post<IManagedMessage>(`/api/guilds/${guildId}/messages`, { channelId, content });
    }

    public importMessage(guildId: string, channelId: string, messageId: string): Observable<IManagedMessage> {
        return this.http.post<IManagedMessage>(`/api/guilds/${guildId}/messages`, { channelId, id: messageId });
    }

    public editMessage(guildId: string, message: IManagedMessage) {
        return this.http.patch(`/api/guilds/${guildId}/banks/${message.id}`, message);
    }

    public removeBank(guildId: string, messageId: number) {
        return this.http.delete(`/api/guilds/${guildId}/banks/${messageId}`);
    }
}
