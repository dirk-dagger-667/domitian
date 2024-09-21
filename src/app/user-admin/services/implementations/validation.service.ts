import { ICustomValidationError } from '../../models/contracts/Icustom-validation-error';
import { CustomValidationError } from '../../models/dtos/custom-validation-error';
import { IValidationService } from '../contracts/ivalidation.service';
import '../../../shared/extensions/form-group-extensions';
import '../../../shared/extensions/string-extensions';
import { isDefined } from '../../../shared/guards/type-guards';
import { FORM_ERRORS } from 'src/app/shared/validators/reg-form-control-errors.provider';
import { Injectable, Inject } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class ValidationService implements IValidationService
{
  constructor(@Inject(FORM_ERRORS) private errors: any) { }

  isControlInvalid(formGroup: FormGroup, controlName: string): boolean
  {
    let control = formGroup.dominitionGetControlInDepth(controlName);
    let isCntrlInvalid = control?.invalid && (control?.dirty || control?.touched);
    let cntrlValRslt = isDefined(isCntrlInvalid) ? isCntrlInvalid : false;

    if (isDefined(control?.parent?.errors))
    {
      let prntInvalid = control?.parent?.errors[controlName];
      
      return isDefined(prntInvalid) ? cntrlValRslt || prntInvalid : cntrlValRslt;
    }

    return cntrlValRslt!;
  }

  contCustValErrorToString(formGroup: FormGroup, cntrlName: string): string
  {
    let isControlInvalid = this.isControlInvalid(formGroup, cntrlName);

    if (!isControlInvalid)
    {
      return '';
    }

    let allCustValErrorsForControl = this.getControlValidationErrors(formGroup, cntrlName);

    let concatControlError = allCustValErrorsForControl
      .map(error => error.value)
      .reduce((acc: any, curr: any) => acc + curr);

    return concatControlError;
  }

  getControlValidationErrors(formmGroup: FormGroup, controlName: string): ICustomValidationError[]
  {
    let allErrors: CustomValidationError[] = this.getCustomFormValidationErrors(formmGroup);

    if (allErrors.length < 0)
    {
      return [];
    }

    let controlErrors = allErrors.filter(error => error.control === controlName || error.error === controlName);

    return controlErrors;
  }

  /*Gets all the validation messages that a GroupForm has and projects them to CustomValidationMessages in a flattened array. */
  getCustomFormValidationErrors(formGroup: FormGroup): ICustomValidationError[]
  {
    let collAllErrors: ICustomValidationError[] = [];

    Object.keys(formGroup.controls).forEach(conKey =>
    {
      let control = formGroup.get(conKey);

      //Iterate over all validation errors and create a CustomvalidationError object that contains the control name, the error type and the value of the error
      if (control?.errors)
      {
        Object.keys(control?.errors).forEach(errorKey => 
        {
          let getError = this.errors[errorKey];
          let error = getError(control?.errors![errorKey]);

          collAllErrors.push(new CustomValidationError(conKey, errorKey, error))
        });
      }

      //Using recursion here to go 1 level deeper into the object if the control is a nested FormGroup to get the errors of the child controls of this nested FromGroup
      if (control instanceof FormGroup)
      {
        let conAsAbsCon: FormGroup = control as FormGroup;

        let conInnerErrors: CustomValidationError[] = this.getCustomFormValidationErrors(conAsAbsCon);

        collAllErrors = [...collAllErrors, ...conInnerErrors];
      }
    });

    return collAllErrors;
  }
}
