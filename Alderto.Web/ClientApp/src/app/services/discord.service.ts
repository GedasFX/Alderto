import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IUser } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class DiscordService {

  constructor(private readonly http: HttpClient) { }

  public getUser(): Observable<IUser> {
    return this.http.get<IUser>(`https://discordapp.com/api/users/@me`);
  }
}
