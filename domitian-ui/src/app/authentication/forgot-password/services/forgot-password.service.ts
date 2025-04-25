import { Injectable } from '@angular/core';
import { patchState, signalState } from '@ngrx/signals';
import { AuthenticationService } from '../../services/authentication.service';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { exhaustMap, pipe, tap } from 'rxjs';
import { tapResponse } from '@ngrx/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { ROUTER_TOKENS } from 'src/app/infrastructure/constants/routing-constants';

export interface ForgotPasswordErrors {
  emailError: string | null;
  resultError: string | null;
}

export interface ForgotPasswordState {
  isLoading: boolean;
  email: string;
  errors: ForgotPasswordErrors;
}

const initialState: ForgotPasswordState = {
  isLoading: false,
  email: '',
  errors: { emailError: null, resultError: null },
};

export
@Injectable({
  providedIn: 'root',
})
class ForgotPasswordService {
  readonly #state = signalState<ForgotPasswordState>(initialState);

  email = this.#state.email;
  emailError = this.#state.errors.resultError;
  resultError = this.#state.errors.resultError;

  constructor(
    private readonly authenticationService: AuthenticationService,
    private readonly router: Router,
    private readonly activatedRoute: ActivatedRoute
  ) {}

  readonly resetPassword = rxMethod<void>(
    pipe(
      tap(() => this.setIsLoading(true)),
      exhaustMap(() => {
        return this.authenticationService
          .resetPassword(this.#state.email())
          .pipe(
            tapResponse({
              next: () => {
                this.router.navigate([
                  '../',
                  ROUTER_TOKENS.CHANGE_PASS_CONFIRMATION,
                  {
                    relativeTo: this.activatedRoute,
                  },
                ]);
              },
              error: (error: HttpErrorResponse) => {
                this.setResultError(
                  'There was an error when trying to reset your password. Try again later.'
                );
              },
              finalize: () => this.setIsLoading(false),
            })
          );
      })
    )
  );

  readonly setIsLoading = (value: boolean): void => {
    patchState(this.#state, { isLoading: value });
  };

  readonly setEmail = (email: string): void => {
    patchState(this.#state, { email: email });
  };

  readonly setResultError = (error: string): void => {
    patchState(this.#state, (state) => ({
      errors: { ...state.errors, resultError: error },
    }));
  };

  readonly setEmailError = (error: string): void => {
    patchState(this.#state, (state) => ({
      errors: { ...state.errors, emailError: error },
    }));
  };
}
