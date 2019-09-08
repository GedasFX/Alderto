import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IGuildChannel } from 'src/app/models';

@Injectable({
  providedIn: 'root'
})
export class AldertoWebGuildApi {
  constructor(private readonly http: HttpClient) { }

  public fetchChannels(id: string): Observable<IGuildChannel[]> {
    return this.http.get<IGuildChannel[]>(`/api/guilds/${id}/channels`);
  }
}
