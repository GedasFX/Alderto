import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { AccountService } from '../services/account.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private readonly accountService: AccountService) { }

  public intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(catchError(err => {
      if (err.status === 401) {
        // Auto logout if 401 response returned from api
        this.accountService.logout();
        // Request a new token.
        this.accountService.loginDiscord().subscribe((u: any) => {
          if (u !== null) {
            location.reload(true);
          }
        });
      }

      return throwError(err);
    }));
  }
}
