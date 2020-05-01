import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { GuildConfiguration } from 'src/app/models';

@Injectable({
  providedIn: 'root'
})
export class AldertoWebGuildPreferencesApi {
  constructor(private readonly http: HttpClient) { }

  public updatePreferences(guildId: string, preferences: GuildConfiguration) {
    return this.http.patch(`/api/guilds/${guildId}/preferences`, preferences);
  }
}
