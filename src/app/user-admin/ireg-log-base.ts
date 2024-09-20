import { Subscription } from "rxjs/internal/Subscription";

export interface IUserAdminBase
{
    readonly subs: Subscription[];
    onBlur($event: FocusEvent): void;
    onFocus($event: FocusEvent): void;
}