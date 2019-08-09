import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GuildService {
  private readonly guildIdSubject: BehaviorSubject<number>;
  public readonly guildId$: Observable<number>;

  constructor() {
    this.guildIdSubject = new BehaviorSubject<number>(0);
    this.guildId$ = this.guildIdSubject.asObservable();
  }

  /**
   * Updates the guild id. Call it when routing to a new page.
   * @param guildId
   */
  public updateGuildId(guildId: number) {
    this.guildIdSubject.next(guildId);
  }
}
