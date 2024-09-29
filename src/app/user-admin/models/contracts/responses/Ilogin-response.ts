export interface LoginResponse 
{
    Email: string | null;
    BearerToken: string | null;
    RefreshToken: string | null;
}