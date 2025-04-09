import { Routes } from '@angular/router';
import { PromotionDashboardComponent } from './promotions/promotion-dashboard/promotion-dashboard.component';
import {
  ROUTER_TOKENS,
  RoutingConstants,
} from './infrastructure/constants/routing-constants';
import { PageNotFoundComponent } from './shared/components/page-not-found/page-not-found.component';
import { authRoutes } from './authentication/auth.routes';
import { AuthWrapperComponent } from './authentication/wrappers/auth-wrapper/auth-wrapper.component';

export const routes: Routes = [
  {
    path: ROUTER_TOKENS.DASHBOARD,
    component: PromotionDashboardComponent,
  },
  {
    path: ROUTER_TOKENS.AUTH,
    component: AuthWrapperComponent,
    children: authRoutes,
  },
  {
    path: RoutingConstants.emptyWildCard,
    redirectTo: ROUTER_TOKENS.AUTH,
    pathMatch: 'full',
  },
  {
    path: RoutingConstants.universalWildCard,
    component: PageNotFoundComponent,
  },
];
