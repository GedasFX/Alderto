import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IGuild } from 'src/app/models';

@Injectable({
  providedIn: 'root'
})
export class AldertoWebUserApi {
  constructor(private readonly http: HttpClient) { }

  public fetchMutualGuilds(guilds: IGuild[]): Observable<IGuild[]> {
    return this.http.post<IGuild[]>('/api/users/@me/mutual-guilds', guilds);
  }
}
