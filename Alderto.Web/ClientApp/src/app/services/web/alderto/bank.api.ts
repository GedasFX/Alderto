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
    return this.http.get<IGuildBank[]>(`/api/bank/list/${guildId}`);
  }

  public createNewBank(guildId: string, name: string): Observable<IGuildBank> {
    return this.http.post<IGuildBank>('/api/bank/create', { guildId, name });
  }
}
