import { Router, RouterLink } from '@angular/router';
import { ROUTER_TOKENS } from 'src/app/infrastructure/constants/routing-constants';
import {
  catchError,
  Observable,
  Subject,
  takeUntil,
  tap,
  throwError,
} from 'rxjs';
import { NgIf } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { RegConfDto } from '../models/dtos/reg-conf-dto';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { InfoSharingService } from 'src/app/core/services/info-sharing/info-sharing.service';
import { AuthenticationService } from '../services/user-admin.service';

@Component({
  selector: 'app-register-confirmation',
  templateUrl: './register-confirmation.component.html',
  styleUrls: ['./register-confirmation.component.css'],
  imports: [RouterLink, NgIf],
})
export class RegisterConfirmationComponent implements OnInit, OnDestroy {
  private readonly sessErrKey: string = 'RegConfError';
  private dto: RegConfDto = new RegConfDto();

  constructor(
    private readonly dataSharingService: InfoSharingService,
    private readonly authenticationService: AuthenticationService,
    private readonly router: Router
  ) {}

  private readonly unsub: Subject<void> = new Subject();

  readonly ROUTER_TOKENS = ROUTER_TOKENS;
  error: string = '';

  ngOnInit(): void {
    this.dataSharingService.data$
      .pipe(
        takeUntil(this.unsub),
        tap((resp) => (this.dto = resp)),
        catchError((err: HttpErrorResponse) => this.handleError(err))
      )
      .subscribe();

    // This is created in anticipation for the full 2-Factor authentication
    // this.subs.push(
    //   this.userAdminService.getConfirmRegistration(this.dto.email)
    //     .subscribe({
    //       next: (resp) => { },
    //       error: (error) => this.handleError(error)
    //     }
    //     ));

    let sesErr = sessionStorage.getItem(this.sessErrKey);

    if (sesErr !== null) {
      this.error = sesErr;
    }
  }

  onClick(): void {
    this.authenticationService
      .get(this.dto.callbackUrl)
      .pipe(
        takeUntil(this.unsub),
        tap(() => this.router.navigate(['../', `${ROUTER_TOKENS.LOGIN}`])),
        catchError((err: HttpErrorResponse) => this.handleError(err))
      )
      .subscribe();
  }

  handleError(err: HttpErrorResponse): Observable<never> {
    sessionStorage.setItem(this.sessErrKey, err.error);
    this.error = err.message;

    return throwError(() => err);
  }

  ngOnDestroy(): void {
    sessionStorage.removeItem(this.sessErrKey);
  }
}
