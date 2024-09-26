import { HttpEvent, HttpHandlerFn, HttpRequest } from "@angular/common/http";
import { inject } from "@angular/core";
import { catchError, switchMap, throwError } from "rxjs";
import { Observable } from "rxjs/internal/Observable";
import { UserAdminService } from "src/app/user-admin/services/user-admin.service";

export function httpTokenHeaderInterceptor(req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>>
{
    let userAdminService = inject(UserAdminService);
    let request = req;

    let accessToken = userAdminService.getAccessToken();

    if (accessToken)
    {
        request = attachTokenToRequest(req, accessToken);
    }

    return next(request).pipe(
        catchError(err =>
        {
            if (err.status === 401)
            {
                // Token might be expired, try refreshing it
                return userAdminService.refreshAccess().pipe(
                    switchMap(() =>
                    {
                        request = attachTokenToRequest(req, userAdminService.getAccessToken());
                        return next(request);
                    }),
                    catchError((refreshError) =>
                    {
                        userAdminService.logout();
                        return throwError(() => new Error(refreshError));
                    })
                );
            }
            return throwError(() => new Error(err));
        })
    );
}

function attachTokenToRequest(httpRequest: HttpRequest<unknown>,
    token: string | null
): HttpRequest<unknown>
{
    let request = httpRequest.clone({
        setHeaders: {
            Authorization: `Bearer ${token}`
        }
    });

    return request;
}