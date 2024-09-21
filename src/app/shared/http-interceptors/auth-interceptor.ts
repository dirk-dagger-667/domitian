import { HttpEvent, HttpHandler, HttpHandlerFn, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { catchError, switchMap, tap, throwError } from "rxjs";
import { Observable } from "rxjs/internal/Observable";
import { IUserAdminService } from "src/app/user-admin/services/contracts/iuser-admin.service";
import { UserAdminService } from "src/app/user-admin/services/implementations/user-admin.service";

@Injectable()
export class AuthInterceptor implements HttpInterceptor
{
    private readonly userAdminService: IUserAdminService = inject(UserAdminService);

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>>
    {
        let accessToken = this.userAdminService.getAccessToken();

        let request = req;
        if (accessToken)
        {
            request = req.clone({
                setHeaders: {
                    Authorization: `Bearer ${accessToken}`
                }
            });
        }

        return next.handle(request).pipe(
            catchError(err =>
            {
                if (err.status === 401)
                {
                    // Token might be expired, try refreshing it
                    return this.userAdminService.refreshAccess().pipe(
                        switchMap(() =>
                        {
                            const newAccessToken = this.userAdminService.getAccessToken();
                            request = req.clone({
                                setHeaders: {
                                    Authorization: `Bearer ${newAccessToken}`
                                }
                            });
                            return next.handle(request);
                        }),
                        catchError((refreshError) =>
                        {
                            this.userAdminService.logout();
                            return throwError(refreshError);
                        })
                    );
                }
                return throwError(err);
            })
        );
    }
}

export function httpTokenHeaderInterceptor(req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>>
{
    let userAdminService = inject(UserAdminService);
    let accessToken = userAdminService.getAccessToken();
    let request = req;

    if (accessToken)
    {
        request = req.clone({
            setHeaders: {
                Authorization: `Bearer ${accessToken}`
            }
        });
    }

    return next(request).pipe(
        // tap({next: (resp: HttpRequest<unknown>) => 
        //     {

        //     }
        // }),
        catchError(err =>
        {
            if (err.status === 401)
            {
                // Token might be expired, try refreshing it
                return userAdminService.refreshAccess().pipe(
                    switchMap(() =>
                    {
                        let newAccessToken = userAdminService.getAccessToken();
                        request = req.clone({
                            setHeaders: {
                                Authorization: `Bearer ${newAccessToken}`
                            }
                        });
                        return next(request);
                    }),
                    catchError((refreshError) =>
                    {
                        userAdminService.logout();
                        return throwError(refreshError);
                    })
                );
            }
            return throwError(err);
        })
    );
}