import { Injectable } from '@angular/core';
import { ConfigProviderService } from '../../services/config-provider/config-provider.service';
import { IAppConfig } from '../../../infrastructure/app-config/iapp-config';
import { catchError, of } from 'rxjs';

@Injectable({providedIn: "root"})
export class UrlPathBuilderService
{

  private data!: IAppConfig | null;

  constructor(private confProvService: ConfigProviderService)
  {
    this.confProvService.data$ !== null 
    ? this.confProvService.data$.pipe(
      catchError(err =>
      {
        console.error(err);
        return of();
      }))
      .subscribe(data => this.data = data) : null;
  }

  //Register controller paths
  private registerPath(): string
  {
    let path = `${this.data?.apiUrlBase}/${this.data?.register.Register}`;

    return path;
  }

  public register(): string 
  {
    let path = `${this.registerPath()}/${this.data?.register.Register}`;

    return path;
  }

  public confirmEmail(): string 
  {
    let path = `${this.registerPath()}/${this.data?.register.ConfirmEmail}`;

    return path;
  }

  public confirmRegistration(): string 
  {
    let path = `${this.registerPath()}/${this.data?.register.ConfirmRegistration}`;

    return path;
  }

  //Login controller paths
  private loginPath(): string
  {
    let path = `${this.data?.apiUrlBase}/${this.data?.login.Login}`;

    return path;
  }

  public login(): string
  {
    let path = `${this.loginPath()}/${this.data?.login.Login}`;

    return path;
  }

  public refreshAccess(): string
  {
    let path = `${this.refreshAccess()}/${this.data?.login.RefreshAccess}`;

    return path;
  }

  public revokeAccess(): string
  {
    let path = `${this.refreshAccess()}/${this.data?.login.RevokeAccess}`;

    return path;
  }
}
