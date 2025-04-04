import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http'
import { catchError, tap } from 'rxjs/operators';
import { Observable, throwError } from 'rxjs';
import { RegisterRequest } from '../models/contracts/requests/register-request';
import { LoginRequest } from '../models/contracts/requests/login-request';
import { AuthConstants } from 'src/app/infrastructure/constants/auth-constants';
import { LoginResponse, loginResponseSchema } from '../models/contracts/responses/Ilogin-response';
import { UrlPathBuilderService } from 'src/app/core/services/url-path-builder/url-path-builder.service';
import { HttpErrorService } from 'src/app/core/services/http-error/http-error.service';

@Injectable({ providedIn: "root" })
export class UserAdminService
{

  constructor(private readonly httpClient: HttpClient,
    private readonly urlPathService: UrlPathBuilderService,
    private readonly errorService: HttpErrorService) { }

  private accessToken: string = '';
  private refreshToken: string = '';

  get<TData>(callbackUrl: string): Observable<TData>
  {
    let response = this.httpClient.get<TData>(callbackUrl, { responseType: 'json' })
      .pipe(
        catchError((err: HttpErrorResponse) => this.handleError(err))
      );

    return response;
  }

  getConfirmRegistration(email: string): Observable<HttpResponse<string>>
  {
    let response = this.httpClient.post<HttpResponse<string>>(this.urlPathService.confirmRegistration(), email, { responseType: "json" })
      .pipe(
        catchError((err: HttpErrorResponse) => this.handleError(err))
      );

    return response;
  }

  register(request: RegisterRequest): Observable<string>
  {
    let response = this.httpClient.post(this.urlPathService.register(), request, { responseType: "text" })
      .pipe(
        catchError((err: HttpErrorResponse) => this.handleError(err))
      );

    return response;
  }

  login(request: LoginRequest): Observable<LoginResponse>
  {
    let response = this.httpClient.post<LoginResponse>(this.urlPathService.login(), request, { responseType: "json" })
      .pipe(
        tap({
          next: (resp: LoginResponse) => {
            let lr = loginResponseSchema.safeParse(resp);

            if(lr.success)
              this.saveTokens(resp);
          },
          error: ((err: HttpErrorResponse) => this.handleError(err))
        })
      );

    return response;
  }

  refreshAccess(): Observable<LoginResponse>
  {
    let response = this.httpClient.post<LoginResponse>(this.urlPathService.refreshAccess(), { responseType: "json" })
      .pipe(
        tap({
          next: (resp: LoginResponse) => {
            
            let lr = loginResponseSchema.safeParse(resp);

            if(lr.success)
              this.saveTokens(resp);
          },
          error: ((err: HttpErrorResponse) => this.handleError(err))
        })
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
            let lr = loginResponseSchema.safeParse(resp);

            if(lr.success) {
              localStorage.removeItem(AuthConstants.AccessToken);
              localStorage.removeItem(AuthConstants.RefreshToken);
            }
          },
          error: ((err: HttpErrorResponse) => this.handleError(err))
        })
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

  private saveTokens(resp: LoginResponse | null): void
  {
    this.accessToken = resp?.bearerToken ?? '';
    this.refreshToken = resp?.refreshToken ?? '';
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
