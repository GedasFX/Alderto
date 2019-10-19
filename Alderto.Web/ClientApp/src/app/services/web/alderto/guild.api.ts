import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IGuildChannel, IGuildRole } from 'src/app/models';

@Injectable({
    providedIn: 'root'
})
export class AldertoWebGuildApi {
    constructor(private readonly http: HttpClient) { }

    public fetchChannels(id: string): Observable<IGuildChannel[]> {
        return this.http.get<IGuildChannel[]>(`/api/guilds/${id}/channels`);
    }

    public fetchRoles(id: string): Observable<IGuildRole[]> {
        return this.http.get<IGuildChannel[]>(`/api/guilds/${id}/roles`);
    }

    public fetchUserRoles(id: string, userId: string = null) {
        return this.http.get<string[]>(`/api/guilds/${id}/users/${userId !== null ? userId : "@me"}`);
    }
}
