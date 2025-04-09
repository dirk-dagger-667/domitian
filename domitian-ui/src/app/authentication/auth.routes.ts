import { Routes } from '@angular/router';
import {
  ROUTER_TOKENS,
  RoutingConstants,
} from '../infrastructure/constants/routing-constants';
import { ChangePasswordConfirmationComponent } from './change-password-confirmation/change-password-confirmation.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { LoginComponent } from './login/login.component';
import { RegisterConfirmationComponent } from './register-confirmation/register-confirmation.component';
import { RegistrationComponent } from './registration/registration.component';

export const authRoutes: Routes = [
  {
    path: ROUTER_TOKENS.LOGIN,
    component: LoginComponent,
  },
  {
    path: ROUTER_TOKENS.REGISTRATION,
    component: RegistrationComponent,
  },
  {
    path: ROUTER_TOKENS.REG_CONFIRMATION,
    component: RegisterConfirmationComponent,
  },
  {
    path: ROUTER_TOKENS.FORGOT_PASS,
    component: ForgotPasswordComponent,
  },
  {
    path: ROUTER_TOKENS.CHANGE_PASS_CONFIRMATION,
    component: ChangePasswordConfirmationComponent,
  },
  {
    path: RoutingConstants.emptyWildCard,
    redirectTo: ROUTER_TOKENS.LOGIN,
    pathMatch: 'full',
  },
];
