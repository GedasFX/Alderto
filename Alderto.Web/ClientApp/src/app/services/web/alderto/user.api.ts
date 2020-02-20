import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IGuild, IUser } from 'src/app/models';

@Injectable({
    providedIn: 'root'
})
export class AldertoWebUserApi {
    constructor(private readonly http: HttpClient) { }

    public fetchUser(): Observable<IUser> {
        return this.http.get<IUser>('/api/users/@me');
    }

    public fetchGuilds(): Observable<IGuild[]> {
        return this.http.get<IGuild[]>('/api/users/@me/guilds');
    }
}
