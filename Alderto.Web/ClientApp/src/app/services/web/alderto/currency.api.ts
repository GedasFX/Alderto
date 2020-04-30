import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ILeaderboardEntry } from 'src/app/models/leaderboard-entry';

@Injectable({
  providedIn: 'root'
})
export class AldertoWebCurrencyApi {
  constructor(private readonly http: HttpClient) { }

  public fetchLeaderboards(guildId: string, take = 100, skip = 0) {
    return this.http.get<ILeaderboardEntry[]>(`/api/guilds/${guildId}/points?take=${take}&skip=${skip}`);
  }
}
