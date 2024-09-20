import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http'
import { UrlPathBuilderService } from 'src/app/shared/services/url-path-builder/url-path-builder.service';
import { catchError, tap } from 'rxjs/operators';
import { HttpErrorService } from 'src/app/shared/utilities/http-error/http-error.service';
import { Observable, throwError } from 'rxjs';
import { RegisterRequest } from '../../models/types/requests/register-request';
import { LoginRequest } from '../../models/types/requests/login-request';
import { IUserAdminService } from '../contracts/iuser-admin.service';
import { AuthConstants } from 'src/app/shared/constants/auth-constants';
import { LoginResponse } from '../../models/types/responses/Ilogin-response';

@Injectable({
  providedIn: 'root'
})
export class UserAdminService implements IUserAdminService
{

  private accessToken: string = '';
  private refreshToken: string = '';

  private httpClient: HttpClient = inject(HttpClient);
  private urlPathService: UrlPathBuilderService = inject(UrlPathBuilderService);
  private errorService: HttpErrorService = inject(HttpErrorService);

  get<TData>(callbackUrl: string): Observable<TData>
  {
    let response = this.httpClient.get<TData>(callbackUrl, { responseType: 'json' })
      .pipe(
        catchError(err => this.handleError(err))
      );

    return response;
  }

  getConfirmRegistration(email: string): Observable<HttpResponse<string>>
  {
    let response = this.httpClient.post<HttpResponse<string>>(this.urlPathService.confirmRegistration(), email, { responseType: "json" })
      .pipe(
        catchError(err => this.handleError(err))
      );

    return response;
  }

  register(request: RegisterRequest): Observable<HttpResponse<string>>
  {
    let response = this.httpClient.post<HttpResponse<string>>(this.urlPathService.register(), request, { responseType: "json" })
      .pipe(
        catchError(err => this.handleError(err))
      );

    return response;
  }

  login(request: LoginRequest): Observable<HttpResponse<LoginResponse>>
  {
    let response = this.httpClient.post<HttpResponse<LoginResponse>>(this.urlPathService.login(), request, { responseType: "json" })
      .pipe(
        tap({
          next: (resp: HttpResponse<LoginResponse>) => this.saveTokens(resp)
        }),
        catchError(err => this.handleError(err))
      );

    return response;
  }

  refreshAccess(): Observable<HttpResponse<LoginResponse>>
  {
    let response = this.httpClient.post<HttpResponse<LoginResponse>>(this.urlPathService.refreshAccess(), { responseType: "json" })
      .pipe(
        tap({
          next: (resp: HttpResponse<LoginResponse>) => this.saveTokens(resp)
        }),
        catchError(err => this.handleError(err))
      );

    return response;
  }

  logout(): Observable<HttpResponse<LoginResponse>>
  {
    let response = this.httpClient.post<HttpResponse<LoginResponse>>(this.urlPathService.revokeAccess(), { responseType: "json" })
      .pipe(
        tap({
          next: (resp: HttpResponse<LoginResponse>) => 
          {
            localStorage.removeItem(AuthConstants.AccessToken);
            localStorage.removeItem(AuthConstants.RefreshToken);
          }
        }),
        catchError(err => this.handleError(err))
      );

    return response;
  }

  getAccessToken(): string | null
  {
    return localStorage.getItem(AuthConstants.AccessToken);
  }

  // This is created in anticipation for the full 2-Factor authentication
  // this.subs.push(
  //   this.userAdminService.getConfirmRegistration(this.dto.email)
  //     .subscribe({
  //       next: (resp) => { },
  //       error: (error) => this.handleError(error)
  //     }
  //     ));

  private saveTokens(resp: any): void
  {
    this.accessToken = resp.accessToken;
    this.refreshToken = resp.refreshToken;
    localStorage.setItem(AuthConstants.AccessToken, this.accessToken);
    localStorage.setItem(AuthConstants.RefreshToken, this.refreshToken);
  }

  private handleError(err: HttpErrorResponse): Observable<never>
  {
    let formattedMessage = this.errorService.formatError(err);

    console.log(err);

    return throwError(() => formattedMessage);
  }
}
