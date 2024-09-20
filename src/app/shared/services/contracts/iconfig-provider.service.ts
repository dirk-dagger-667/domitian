import { Observable } from "rxjs";
import { IAppConfig } from "../../app-config/iapp-config";

export interface IConfigProviderService {
    readonly data$: Observable<IAppConfig | null>;

    fetchConfig(): void;
}