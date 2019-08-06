import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IDiscordUser } from '../models/discord_user';

@Injectable({
  providedIn: 'root'
})
export class DiscordService {

  constructor(private readonly http: HttpClient) { }

  public getUser(): Observable<IDiscordUser> {
    return this.http.get<IDiscordUser>(`https://discordapp.com/api/users/@me`);
  }
}
