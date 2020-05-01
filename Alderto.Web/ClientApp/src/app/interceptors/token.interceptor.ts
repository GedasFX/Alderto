import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, switchMap, filter, take } from 'rxjs/operators';

import { AccountService } from '../services/account.service';

@Injectable({
  providedIn: 'root'
})
export class TokenInterceptor implements HttpInterceptor {
  constructor(private readonly accountService: AccountService) { }

  private tokenRefreshing = false;

  public intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (this.accountService && this.accountService.accessToken) {

      // Ensure request is going to the API.
      if (request.url.startsWith('/api')) {
        request = this.addToken(request, this.accountService.accessToken);
      }
    }

    return next.handle(request).pipe(catchError(error => {

      // Catch 401 Unauthorized errors
      if (error instanceof HttpErrorResponse && error.status === 401) {

        // Ensure request is going to the API.
        if (request.url.startsWith('/api')) {
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
      this.accountService.user$.next(null);

      return this.accountService.renewToken().pipe(
        switchMap(user => {
          this.tokenRefreshing = false;
          return next.handle(this.addToken(request, user.access_token));
        }));
    } else {
      // Token is refreshing. Wait for the refresh token to appear.
      return this.accountService.user$.pipe(
        filter(user => user != null), take(1), // This is effectively await token.
        switchMap(user => {
          return next.handle(this.addToken(request, user.access_token));
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
