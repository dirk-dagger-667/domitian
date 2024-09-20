export interface IAppConfig {
    apiUrlBase : string,
    register: {
        basePath : string,
        ConfirmEmail : string,
        Register : string,
        ConfirmRegistration : string
    },
    login: {
        basePath: string,
        Login: string,
        RefreshAccess: string,
        RevokeAccess: string
    }
}
