import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandlerFn,
  HttpRequest,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError } from 'rxjs';
import { Observable } from 'rxjs/internal/Observable';
import { AuthenticationService } from 'src/app/authentication/services/user-admin.service';

export function httpTokenHeaderInterceptor(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> {
  let authenticationService = inject(AuthenticationService);
  let request = req;

  let accessToken = authenticationService.getAccessToken();

  if (accessToken) {
    request = attachTokenToRequest(req, accessToken);
  }

  return next(request).pipe(
    catchError((err: HttpErrorResponse) => {
      if (err.status === 401) {
        // Token might be expired, try refreshing it
        return authenticationService.refreshAccess().pipe(
          switchMap(() => {
            request = attachTokenToRequest(
              req,
              authenticationService.getAccessToken()
            );
            return next(request);
          }),
          catchError((refreshError) => {
            authenticationService.logout();
            return throwError(() => refreshError);
          })
        );
      } else if (err.status >= 500) {
        const errType = err.type;
      }
      return throwError(() => err);
    })
  );
}

function attachTokenToRequest(
  httpRequest: HttpRequest<unknown>,
  token: string | null
): HttpRequest<unknown> {
  let request = httpRequest.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`,
    },
  });

  return request;
}
