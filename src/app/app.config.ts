import { APP_INITIALIZER, ApplicationConfig, importProvidersFrom } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { provideRouter } from "@angular/router";
import { routes } from "./app.routes";
import { provideHttpClient, withInterceptors } from "@angular/common/http";
import { take } from "rxjs";
import { httpTokenHeaderInterceptor } from "./infrastructure/http-interceptors/auth-interceptor";
import { ConfigProviderService } from "./core/services/config-provider/config-provider.service";

export function setupAppConfigServiceFactory(
    service: ConfigProviderService
)
{
    return () => 
    {
        service.fetchConfig();
        return service.data$?.pipe(take(1));
    }
}

export const appConfig: ApplicationConfig = {
    providers: [
        provideHttpClient(
            withInterceptors([httpTokenHeaderInterceptor])
        ),
        importProvidersFrom(
            FormsModule
        ),
        provideRouter(routes),
        {
            provide: APP_INITIALIZER,
            useFactory: setupAppConfigServiceFactory,
            multi: true,
            deps: [ConfigProviderService]
        }
    ]
};