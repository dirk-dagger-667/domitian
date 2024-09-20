import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { IAppConfig } from "../../app-config/iapp-config";
import { BehaviorSubject, filter, map, Observable, shareReplay } from "rxjs";
import { IConfigProviderService } from "../contracts/iconfig-provider.service";

@Injectable({
    providedIn: 'root'
})
export class ConfigProviderService implements IConfigProviderService
{
    private config = new BehaviorSubject<IAppConfig | null>(null);
    private confilFilePath = 'assets/app.config.json';
    private httpClient: HttpClient = inject(HttpClient);
    readonly data$: Observable<IAppConfig | null> = this.config.asObservable()
        .pipe(
            filter((config) => !!config),
            map((config) => config),
            shareReplay(1)
        );

    get appConfig()
    {
        return this.config.getValue();
    }

    fetchConfig(): void
    {
        this.httpClient.get<IAppConfig>(this.confilFilePath)
            .subscribe(
                {
                    next: (config) => this.config.next(config),
                    error: () => this.config.next({
                        apiUrlBase: '',
                        register: {
                            basePath: '',
                            ConfirmEmail: '',
                            ConfirmRegistration: '',
                            Register: ''
                        },
                        login: {
                            basePath: '',
                            Login: '',
                            RefreshAccess: '',
                            RevokeAccess: ''
                        }
                    }),
                });
    }
}