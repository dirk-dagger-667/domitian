import { AbstractControl } from "@angular/forms";

export function changePlaceholderOnBlur($event: FocusEvent, cntrl: AbstractControl | null, cntrlPlcHolder: string)
{
    if ($event.target instanceof HTMLInputElement)
    {
        let control = $event.target as HTMLInputElement;
        
        if (!!cntrl?.value){
            cntrl?.setValue(control.value === cntrlPlcHolder ? "" : control.value);
        }
    }
}

export function removeValueOnFocus($event: FocusEvent, cntrlPlcHolder: string): void
{
    if ($event.target instanceof HTMLInputElement) 
    {
        let control = $event.target as HTMLInputElement;

        if (!!control.value)
        {
            control.value = (control.value.toLocaleLowerCase() === cntrlPlcHolder) ? '' : control.value;
        }
    }
}