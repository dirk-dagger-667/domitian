export abstract class RoutingConstants 
{
    public static emptyWildCard: string = '';
    public static universalWildCard: string = '**';
}

export enum ROUTER_TOKENS 
{
    API_BASE = '/api',
    LOGIN = '/login',
    REGISTRATION = '/registration',
    REG_CONFIRMATION = '/registerConfirmation',
    FORGOT_PASS = '/forgotPassword',
    CHANGE_PASS_CONFIRMATION = '/changePasswordConfirmation',
    DASHBOARD = '/dashboard',
}