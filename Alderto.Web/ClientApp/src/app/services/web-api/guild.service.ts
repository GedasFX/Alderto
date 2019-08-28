import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { NavigationService } from '../navigation.service';
import { IGuildChannel } from 'src/app/models';

@Injectable({
  providedIn: 'root'
})
export class GuildService {
  constructor(
    private readonly http: HttpClient,
    private readonly nav: NavigationService) { }

  public fetchCurrentChannels(): Observable<IGuildChannel[]> {
    return this.http.get<IGuildChannel[]>(`/api/guild/${this.nav.getCurrentGuildId()}/channels`);
  }

  public fetchChannels(id: string): Observable<IGuildChannel[]> {
    return this.http.get<IGuildChannel[]>(`/api/guild/${id}/channels`);
  }
}
