import { Observable } from "rxjs";
import { RegisterRequest } from "../../models/types/requests/register-request";
import { LoginRequest } from "../../models/types/requests/login-request";
import { HttpResponse } from "@angular/common/http";
import { LoginResponse } from "../../models/types/responses/Ilogin-response";

export interface IUserAdminService
{
    get<TData>(callbackUrl: string): Observable<TData>;
    getConfirmRegistration(email: string): Observable<HttpResponse<string>>;
    register(request: RegisterRequest): Observable<HttpResponse<string>>;
    login(request: LoginRequest): Observable<HttpResponse<LoginResponse>>;
    refreshAccess(): Observable<HttpResponse<LoginResponse>>;
    logout(): Observable<HttpResponse<LoginResponse>>;
    getAccessToken(): string | null;
}