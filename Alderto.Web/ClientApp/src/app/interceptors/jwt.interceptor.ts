import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';

import { AccountService } from '../services';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private readonly accountService: AccountService) { }

  public intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const currentUser = this.accountService.user;
    if (currentUser) {
      if (request.url.startsWith('/api/')) {
        // Going to the API

        request = request.clone({
          setHeaders: {
            Authorization: `Bearer ${currentUser.token}`
          }
        });

      } else if (request.url.startsWith('https://discordapp.com/api/')) {
        // Going to Discord API

        request = request.clone({
          setHeaders: {
            Authorization: `Bearer ${currentUser.discord}`
          }
        });

      }
    }

    return next.handle(request);
  }
}
