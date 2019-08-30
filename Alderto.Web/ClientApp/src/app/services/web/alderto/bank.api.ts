import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { IGuildBank } from 'src/app/models';
import { catchError, map, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AldertoWebBankApi {
  constructor(private readonly http: HttpClient) { }

  public fetchBanks(guildId: string): Observable<IGuildBank[]> {
    return this.http.get<IGuildBank[]>(`/api/bank/list/${guildId}`);
  }

  public createNewBank(guildId: string, name: string, logChannelId: string) {
    return this.http.post('/api/bank/create', { guildId, name, logChannelId })
      .pipe(tap(error => {
        console.log(typeof(error));
        console.log(error);
        return throwError(error);
      }));
  }

  public removeBank(guildId: string, bankId: number): Observable<void> {
    return this.http.delete<void>(`/api/bank/remove/${guildId}/${bankId}`);
  }
}
