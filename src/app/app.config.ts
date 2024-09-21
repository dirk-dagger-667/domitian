import { APP_INITIALIZER, ApplicationConfig, importProvidersFrom } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { provideRouter } from "@angular/router";
import { routes } from "./app.routes";
import { provideHttpClient } from "@angular/common/http";
import { ConfigProviderService } from "./shared/services/config-provider/config-provider.service";
import { take } from "rxjs";

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
        provideHttpClient(),
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