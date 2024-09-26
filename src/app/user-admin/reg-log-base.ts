import { Injectable, OnDestroy } from "@angular/core";
import { ValidatorConstants } from "../infrastructure/constants/validation-constants";
import { Subscription } from "rxjs/internal/Subscription";

@Injectable({ providedIn: "root" })
export abstract class UserAdminBase implements OnDestroy
{
    readonly subs: Subscription[] = [];

    onBlur($event: FocusEvent)
    {
        if ($event.target instanceof HTMLInputElement)
        {
            let control = $event.target as HTMLInputElement;

            if (!control.value || control.value.trim() === '')
            {
                control.value = control.id === ValidatorConstants.emailControlName ? ValidatorConstants.mailPlaceholder : '';
            }
        }
    }

    onFocus($event: FocusEvent): void
    {
        if ($event.target instanceof HTMLInputElement) 
        {
            let control = $event.target as HTMLInputElement;

            if (control.value)
            {
                control.value = (control.value.toLocaleLowerCase() === ValidatorConstants.mailPlaceholder) ? '' : control.value;
            }
        }
    }

    ngOnDestroy(): void
    {
        this.subs.forEach(sub => 
        {
            if (sub !== undefined) 
            {
                sub.unsubscribe();
            }
        });
    }
}