import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IUser } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class DiscordService {

  constructor(private readonly http: HttpClient) { }

  public fetchUser(userId?: number): Observable<IUser> {
    if (userId == null) {
      return this.http.get<IUser>('https://discordapp.com/api/users/@me');
    }
    this.http.get<IUser>(`https://discordapp.com/api/users/${userId}`);
  }
}
