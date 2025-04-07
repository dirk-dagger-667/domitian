export interface IAppConfig {
    apiUrlBase : string,
    register: {
        BasePath: string,
        ConfirmEmail : string,
        Register : string,
        ConfirmRegistration : string
    },
    login: {
        BasePath: string,
        Login: string,
        RefreshAccess: string,
        RevokeAccess: string
    }
}
