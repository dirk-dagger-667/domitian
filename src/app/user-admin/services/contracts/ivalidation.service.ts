import { FormGroup } from "@angular/forms";
import { ICustomValidationError } from "../../models/contracts/Icustom-validation-error";

export interface IValidationService
{
    contCustValErrorToString(formGroup: FormGroup<any>, controlName: string): string

    isControlInvalid(formGroup: FormGroup<any>, controlName: string): boolean;

    getControlValidationErrors(fromGroup: FormGroup<any>, controlName: string): ICustomValidationError[];
}