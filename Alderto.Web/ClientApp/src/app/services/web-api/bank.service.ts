import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IGuildBank } from '../../models';
import { NavigationService } from '../navigation.service';

@Injectable({
  providedIn: 'root'
})
export class GuildBankService {
  constructor(
    private readonly http: HttpClient,
    private readonly nav: NavigationService) { }

  public fetchBanks(): Observable<IGuildBank[]> {
    return this.http.get<IGuildBank[]>(`/api/bank/list?guildId=${this.nav.getCurrentGuildId()}`);
  }

  public createNewBank(name: string): Observable<IGuildBank> {
    return this.http.post<IGuildBank>('/api/bank/create', { guildId: this.nav.getCurrentGuildId(), name });
  }
}
