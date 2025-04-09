import { Router, RouterLink } from '@angular/router';
import { ROUTER_TOKENS } from 'src/app/infrastructure/constants/routing-constants';
import { Observable, Subscription, throwError } from 'rxjs';
import { NgIf } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { RegConfDto } from '../models/dtos/reg-conf-dto';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { InfoSharingService } from 'src/app/core/services/info-sharing/info-sharing.service';
import { UserAdminService } from '../services/user-admin.service';
import { AuthWrapperComponent } from '../wrappers/auth-wrapper/auth-wrapper.component';

@Component({
  selector: 'app-register-confirmation',
  templateUrl: './register-confirmation.component.html',
  styleUrls: ['./register-confirmation.component.css'],
  imports: [AuthWrapperComponent, RouterLink, NgIf],
})
export class RegisterConfirmationComponent implements OnInit, OnDestroy {
  private readonly sessErrKey: string = 'RegConfError';
  private dto: RegConfDto = new RegConfDto();

  constructor(
    private readonly dataSharingService: InfoSharingService,
    private readonly userAdminService: UserAdminService,
    private readonly router: Router
  ) {}

  private readonly subs: Subscription[] = [];

  readonly ROUTER_TOKENS = ROUTER_TOKENS;
  error: string = '';

  ngOnInit(): void {
    this.subs.push(
      this.dataSharingService.data$.subscribe({
        next: (resp) => {
          this.dto = resp;
        },
        error: (error) => this.handleError(error),
      })
    );

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
    this.subs.push(
      this.userAdminService.get(this.dto.callbackUrl).subscribe({
        next: () => {
          this.router.navigate(['../', `${ROUTER_TOKENS.LOGIN}`]);
        },
        error: (error: any) => this.handleError(error),
      })
    );
  }

  handleError(err: HttpErrorResponse): Observable<never> {
    sessionStorage.setItem(this.sessErrKey, err.error);
    this.error = err.message;

    return throwError(() => err);
  }

  ngOnDestroy(): void {
    sessionStorage.removeItem(this.sessErrKey);

    this.subs.forEach((sub) => {
      if (sub !== undefined) {
        sub.unsubscribe();
      }
    });
  }
}
