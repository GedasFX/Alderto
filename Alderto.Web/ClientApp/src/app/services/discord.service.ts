import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DiscordUser } from '../models/discord_user';

@Injectable({
  providedIn: 'root'
})
export class DiscordService {

  constructor(private readonly http: HttpClient) { }

  public getUser(): Observable<DiscordUser> {
    return this.http.get<DiscordUser>(`https://discordapp.com/api/users/@me`);
  }
}
