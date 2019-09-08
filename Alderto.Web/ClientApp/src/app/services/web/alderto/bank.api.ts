import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IGuildBank, IGuildBankItem } from 'src/app/models';

@Injectable({
  providedIn: 'root'
})
export class AldertoWebBankApi {
  constructor(private readonly http: HttpClient) { }

  public fetchBanks(guildId: string): Observable<IGuildBank[]> {
    return this.http.get<IGuildBank[]>(`/api/guilds/${guildId}/banks`);
  }

  public createNewBank(guildId: string, bank: IGuildBank): Observable<IGuildBank> {
    return this.http.post<IGuildBank>(`/api/guilds/${guildId}/banks`, bank);
  }

  public editBank(guildId: string, bankId: number, name: string, logChannelId: string) {
    return this.http.patch(`/api/guilds/${guildId}/banks/${bankId}`, { name, logChannelId });
  }

  public removeBank(guildId: string, bankId: number) {
    return this.http.delete(`/api/guilds/${guildId}/banks/${bankId}`);
  }

  public createNewBankItem(guildId: string, bankId: number, item: IGuildBankItem) {
    return this.http.post<IGuildBankItem>(`/api/guilds/${guildId}/banks/${bankId}/items`, item);
  }

  public editBankItem(guildId: string, bankId: number, itemId: number, item: IGuildBankItem) {
    return this.http.patch(`/api/guilds/${guildId}/banks/${bankId}/items/${itemId}`, item);
  }
}
