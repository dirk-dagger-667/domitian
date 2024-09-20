import { Routes } from "@angular/router";
import { PromotionDashboardComponent } from "./promotions/promotion-dashboard/promotion-dashboard.component";
import { ChangePasswordConfirmationComponent } from "./user-admin/change-password-confirmation/change-password-confirmation.component";
import { ForgotPasswordComponent } from "./user-admin/forgot-password/forgot-password.component";
import { LoginComponent } from "./user-admin/login/login.component";
import { RegisterConfirmationComponent } from "./user-admin/register-confirmation/register-confirmation/register-confirmation.component";
import { RegistrationComponent } from "./user-admin/registration/registration.component";
import { PageNotFoundComponent } from "./shared/components/page-not-found/page-not-found/page-not-found.component";
import { ROUTER_TOKENS, RoutingConstants } from "./shared/constants/routing-constants";

export const routes: Routes = [
    { path: ROUTER_TOKENS.LOGIN, component: LoginComponent },
    { path: ROUTER_TOKENS.REGISTRATION, component: RegistrationComponent },
    { path: ROUTER_TOKENS.FORGOT_PASS, component: ForgotPasswordComponent },
    { path: ROUTER_TOKENS.CHANGE_PASS_CONFIRMATION, component: ChangePasswordConfirmationComponent },
    { path: ROUTER_TOKENS.DASHBOARD, component: PromotionDashboardComponent },
    { path: ROUTER_TOKENS.REG_CONFIRMATION, component: RegisterConfirmationComponent },
    { path: RoutingConstants.emptyWildCard, redirectTo: ROUTER_TOKENS.LOGIN, pathMatch: 'full' },
    { path: RoutingConstants.universalWildCard, component: PageNotFoundComponent },
];