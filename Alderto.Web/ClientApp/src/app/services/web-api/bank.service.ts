import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { NavigationService } from '../navigation.service';
import { IGuildBank } from 'src/app/models';

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
