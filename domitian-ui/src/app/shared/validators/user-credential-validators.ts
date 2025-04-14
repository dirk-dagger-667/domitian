import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function passwordValidator(passwordRegex: RegExp): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    let forbidden = passwordRegex.test(control.value);
    return forbidden ? null : { passRegValidator: { value: control.value } };
  };
}

export function MustMatch(
  firstCntrlName: string,
  secondCntrlName: string
): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    let firstValue = control.get(firstCntrlName)?.value;
    let secondValue = control.get(secondCntrlName)?.value;

    if (firstValue !== secondValue) {
      //The key should have the same value as the control name(see ValidatorConstants)
      return { notMatch: true };
    }

    return null;
  };
}
