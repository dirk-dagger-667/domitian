import { Injectable } from '@angular/core';
import { patchState, signalState } from '@ngrx/signals';
import { AuthenticationService } from '../../services/authentication.service';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { exhaustMap, pipe, tap } from 'rxjs';
import { tapResponse } from '@ngrx/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { ROUTER_TOKENS } from 'src/app/infrastructure/constants/routing-constants';
import { InfoSharingService } from 'src/app/core/services/info-sharing/info-sharing.service';

export interface RegistrationErrors {
  emailError: string | null;
  passwordError: string | null;
  confPassError: string | null;
  registrationError: string | null;
}

export interface RegistrationState {
  isLoading: boolean;
  email: string;
  password: string;
  confPassword: string;
  errors: RegistrationErrors;
}

const initialState: RegistrationState = {
  isLoading: false,
  email: '',
  password: '',
  confPassword: '',
  errors: {
    emailError: null,
    passwordError: null,
    confPassError: null,
    registrationError: null,
  },
};

@Injectable({
  providedIn: 'root',
})
export class RegistrationService {
  readonly #state = signalState(initialState);

  emailError = this.#state.email;
  passwordError = this.#state.email;
  confPasswordError = this.#state.email;

  constructor(
    private readonly authenticationService: AuthenticationService,
    private readonly infoShareingService: InfoSharingService,
    private readonly router: Router
  ) {}

  readonly register = rxMethod<void>(
    pipe(
      tap(() => this.setIsLoading(true)),
      exhaustMap(() => {
        return this.authenticationService
          .register({
            Email: this.#state.email(),
            Password: this.#state.password(),
            ConfirmPassword: this.#state.confPassword(),
          })
          .pipe(
            tapResponse({
              next: (resp: string) => {
                this.infoShareingService.sendData({
                  callbackUrl: resp,
                  email: this.#state.email(),
                });

                this.router.navigate([
                  '../',
                  `${ROUTER_TOKENS.REG_CONFIRMATION}`,
                ]);
              },
              error: (error: HttpErrorResponse) => {
                this.setRegistrationError(
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

  readonly setPassword = (password: string): void => {
    patchState(this.#state, { password: password });
  };

  readonly setConfPassword = (confPassword: string): void => {
    patchState(this.#state, { confPassword: confPassword });
  };

  readonly setEmailError = (error: string): void => {
    patchState(this.#state, (state) => ({
      errors: { ...state.errors, emailError: error },
    }));
  };

  readonly setPasswordError = (error: string): void => {
    patchState(this.#state, (state) => ({
      errors: { ...state.errors, passwordError: error },
    }));
  };

  readonly setConfPassError = (error: string): void => {
    patchState(this.#state, (state) => ({
      errors: { ...state.errors, confPassError: error },
    }));
  };

  readonly setRegistrationError = (error: string): void => {
    patchState(this.#state, (state) => ({
      errors: { ...state.errors, registrationError: error },
    }));
  };
}
