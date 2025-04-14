import { InjectionToken } from '@angular/core';

export const defaultErrors = {
  required: (value: any) => `This field is required. \n`,
  maxlength: ({ requiredLength, actualLength }: any) =>
    `Expected length should be less than ${requiredLength} chars but is ${actualLength} chars long. \n`,
  minlength: ({ requiredLength, actualLength }: any) =>
    `Expected length should be more than ${requiredLength} chars but is ${actualLength} chars long. \n`,
  email: (value: any) =>
    'Invalid email format. Please provide a valid email. \n',
  passRegValidator: (value: any) =>
    'Password should: \n 1. be >= 8 chars long \n 2. be =< 24 chars long 3. contain at least 1 uppercase letter \n 3. contain at least 1 lowercase letter \n 4. contain at least 1 number from 1 to 9 \n 5. contain at least 1 special character(@, $, !, %, *, ?, &) \n',
  confirmPassword: (value: any) =>
    'This field should have the the same value as the password field. \n',
};

export const LOGIN_FORM_ERRORS = new InjectionToken('FORM_ERRORS', {
  providedIn: 'root',
  factory: () => defaultErrors,
});
