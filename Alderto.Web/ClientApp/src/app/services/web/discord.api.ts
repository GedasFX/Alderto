import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IUser, IGuild } from 'src/app/models';

@Injectable({
  providedIn: 'root'
})
export class DiscordWebApi {

  constructor(private readonly http: HttpClient) { }

  public fetchUser(): Observable<IUser> {
    return this.http.get<IUser>('https://discordapp.com/api/users/@me');
  }

  public fetchGuilds(): Observable<IGuild[]> {
    return this.http.get<IGuild[]>('https://discordapp.com/api/users/@me/guilds');
  }
}
