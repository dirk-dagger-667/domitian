import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";
import { ValidatorConstants } from "../constants/validation-constants";

export function passwordValidator(passwordRegex: RegExp): ValidatorFn
{
    return (control: AbstractControl): ValidationErrors | null =>
    {
        let forbidden = passwordRegex.test(control.value);
        return forbidden ? null : { passRegValidator: { value: control.value } };
    };
}

export function confirmPasswordSameValidator(control: AbstractControl): ValidationErrors | null
{
    let password = control.get(ValidatorConstants.passwordControlName);
    let confirmPassword = control.get(ValidatorConstants.confirmPasswordControlName);
    
    if (password?.value !== confirmPassword?.value)
    {
        //The key should have the same value as the control name(see ValidatorConstants)
        return { confirmPassword : true };
    }

    return null;
}