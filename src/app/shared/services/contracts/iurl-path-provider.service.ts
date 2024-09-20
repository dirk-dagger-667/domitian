export interface IUrlPathBuilderService
{
    //Register controller paths
    register(): string;
    confirmEmail(): string;
    confirmRegistration(): string;

    //Login controller paths
    login(): string;
    refreshAccess(): string;
    revokeAccess(): string;
}