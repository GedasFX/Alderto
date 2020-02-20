import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, switchMap, filter, take } from 'rxjs/operators';

import { AccountService } from '../services/account.service';
import { ITokenResponse } from '../services';

@Injectable({
  providedIn: 'root'
})
export class TokenInterceptor implements HttpInterceptor {
  constructor(private readonly accountService: AccountService) { }

  private tokenRefreshing = false;

  public intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (this.accountService && this.accountService.accessToken) {

      // Ensure request is going to the API.
      if (request.url.startsWith(`/api/`)) {
        request = this.addToken(request, this.accountService.accessToken);
      }
    }

    return next.handle(request).pipe(catchError(error => {

      // Catch 401 Unauthorized errors
      if (error instanceof HttpErrorResponse && error.status === 401) {

        // Ensure request is going to the API.
        if (window.location.origin + request.urlWithParams === error.url) {
          // JWT Expired. Refresh it.
          return this.onTokenExpired(request, next);
        }
      }

      return throwError(error);
    }));
  }

  private onTokenExpired(request: HttpRequest<any>, next: HttpHandler) {
    if (!this.tokenRefreshing) {
      this.tokenRefreshing = true;
      this.accountService.accessToken$.next(null);

      return this.accountService.refreshSession().pipe(
        switchMap((tokens: ITokenResponse) => {
          this.accountService.storeTokens(tokens);
          this.tokenRefreshing = false;
          return next.handle(this.addToken(request, tokens.access_token));
        }));

    } else {
      // Token is refreshing. Wait for the refresh token to appear.
      return this.accountService.accessToken$.pipe(
        filter(token => token != null), take(1), // This is effectively await token.
        switchMap(token => {
          return next.handle(this.addToken(request, token as string));
        }));
    }
  }

  private addToken(request: HttpRequest<any>, token: string) {
    return request.clone({
      setHeaders: {
        'Authorization': `Bearer ${token}`,
      },
    });
  }
}
