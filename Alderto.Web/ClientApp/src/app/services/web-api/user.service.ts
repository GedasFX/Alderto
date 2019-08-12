import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IGuild } from '../../models';

@Injectable({
  providedIn: 'root'
})
export class WebUserService {
  constructor(private readonly http: HttpClient) { }

  public getMutualGuilds(guilds: IGuild[]): Observable<IGuild[]> {
    return this.http.post<IGuild[]>('/api/user/guilds', guilds);
  }
}
