import { ICustomValidationError } from "../contracts/Icustom-validation-error";

export class CustomValidationError implements ICustomValidationError
{
    public control: string;
    public error: string;
    public value: any;

    constructor(control: string, error: string, value: any)
    {
        this.control = control;
        this.error = error;
        this.value = value;
    }
}