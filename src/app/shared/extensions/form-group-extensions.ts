import { AbstractControl, FormGroup} from "@angular/forms";

declare module '@angular/forms' {
    interface FormGroup
    {
        dominitionGetControlInDepth(controlName: string): AbstractControl<any, any> | null;
    }
}

FormGroup.prototype.dominitionGetControlInDepth = function (controlName: string): AbstractControl<any, any> | null
{
    let control = this.get(controlName);

    if (!control)
    {
        Object.keys(this.controls).forEach((key: string) =>
        {
            let absControl = this.get(key);

            if (absControl instanceof FormGroup)
            {
                var absControlAsFormGroup = absControl as FormGroup<any>;

                control = absControlAsFormGroup.dominitionGetControlInDepth(controlName)
            }
        });
    }

    return control;
}