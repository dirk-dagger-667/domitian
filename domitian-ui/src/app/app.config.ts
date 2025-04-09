import {
  ApplicationConfig,
  importProvidersFrom,
  inject,
  provideAppInitializer,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { provideRouter, withDebugTracing } from '@angular/router';
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { take } from 'rxjs';
import { httpTokenHeaderInterceptor } from './infrastructure/http-interceptors/auth-interceptor';
import { ConfigProviderService } from './core/services/config-provider/config-provider.service';

const setupAppConfigServiceFactory = () => {
  return () => {
    const service = inject(ConfigProviderService);
    service.fetchConfig();
    return service.data$?.pipe(take(1));
  };
};

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withInterceptors([httpTokenHeaderInterceptor])),
    importProvidersFrom(FormsModule),
    provideRouter(routes, withDebugTracing()),
    provideAppInitializer(setupAppConfigServiceFactory()),
  ],
};
