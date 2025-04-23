import { Injectable, signal } from '@angular/core';
import { patchState, signalState } from '@ngrx/signals';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { exhaustMap, pipe, tap, throwError } from 'rxjs';
import { AuthenticationService } from '../../services/user-admin.service';
import { tapResponse } from '@ngrx/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { ROUTER_TOKENS } from 'src/app/infrastructure/constants/routing-constants';
import { HttpErrorResponse } from '@angular/common/http';

export interface LoginErrors {
  emailError: string | null;
  passwordError: string | null;
  resultError: string | null;
}

export interface LoginState {
  isLoading: boolean;
  email: string;
  password: string;
  rememberMe: boolean;
  errors: LoginErrors;
}

const initialState = {
  isLoading: false,
  email: '',
  password: '',
  rememberMe: false,
  errors: {
    emailError: null,
    passwordError: null,
    resultError: null,
  },
};

@Injectable({ providedIn: 'root' })
export class LoginService {
  readonly #state = signalState<LoginState>(initialState);
  readonly ROUTER_TOKENS = ROUTER_TOKENS;

  constructor(
    private readonly authenticationService: AuthenticationService,
    private readonly router: Router,
    private readonly activatedRoute: ActivatedRoute
  ) {}

  mailError = this.#state.errors.emailError;
  passwordError = this.#state.errors.passwordError;
  loginError = this.#state.errors.resultError;

  readonly login = rxMethod<void>(
    pipe(
      tap(() => this.setIsLoading(true)),
      exhaustMap(() => {
        return this.authenticationService
          .login({
            email: this.#state.email(),
            password: this.#state.password(),
            rememberMe: this.#state.rememberMe(),
          })
          .pipe(
            tapResponse({
              next: () => {
                this.setIsLoading(false);

                this.router.navigate([
                  '../',
                  ROUTER_TOKENS.DASHBOARD,
                  {
                    relativeTo: this.activatedRoute,
                  },
                ]);
              },
              error: (error: HttpErrorResponse) => {
                this.setLoginError(
                  'There was an error when trying to login. Try again later.'
                );

                return throwError(() => error);
              },
            })
          );
      })
    )
  );

  readonly setLoginError = (error: string): void => {
    patchState(this.#state, (state) => ({
      errors: { ...state.errors, resultError: error },
    }));
  };

  readonly setEmialError = (error: string): void => {
    patchState(this.#state, (state) => ({
      errors: { ...state.errors, emailError: error },
    }));
  };

  readonly setPasswordError = (error: string): void => {
    patchState(this.#state, (state) => ({
      errors: { ...state.errors, passwordError: error },
    }));
  };

  readonly setRembemberMe = (value: boolean): void => {
    patchState(this.#state, { rememberMe: value });
  };

  readonly setEmail = (value: string): void => {
    patchState(this.#state, { email: value });
  };

  readonly setPassword = (value: string): void => {
    patchState(this.#state, { password: value });
  };

  readonly setIsLoading = (value: boolean): void => {
    patchState(this.#state, { isLoading: value });
  };
}
