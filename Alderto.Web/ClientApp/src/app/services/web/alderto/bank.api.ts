import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IGuildBank } from 'src/app/models';

@Injectable({
  providedIn: 'root'
})
export class AldertoWebBankApi {
  constructor(private readonly http: HttpClient) { }

  public fetchBanks(guildId: string): Observable<IGuildBank[]> {
    return this.http.get<IGuildBank[]>(`/api/guilds/${guildId}/banks`);
  }

  public createNewBank(guildId: string, name: string, logChannelId: string): Observable<IGuildBank> {
    return this.http.post<IGuildBank>(`/api/guilds/${guildId}/banks`, { name, logChannelId });
  }

  public editBank(guildId: number, bankId: number, name: string, logChannelId: string) {
    return this.http.patch(`/api/guilds/${guildId}/banks/${bankId}`, { name, logChannelId });
  }

  public removeBank(guildId: string, bankId: number) {
    return this.http.delete(`/api/guilds/${guildId}/banks/${bankId}`);
  }
}
