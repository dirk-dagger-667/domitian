import { ValidatorConstants } from "src/app/infrastructure/constants/validation-constants";
import { TITDto, TITEvents } from "src/app/shared/contracts/titdto";
import { Validators } from "@angular/forms";

export const emptyTITDto: TITDto = createEmptyTitDto();

export const emailTITDto: TITDto = {
    inputType: 'text',
    id: '',
    formCntrlName: ValidatorConstants.emailControlName,
    title: 'Email',
    cntrlErrMsg: '',

    events: {
        onFocus: ($event: FocusEvent): void => { },
        onBlur: ($event: FocusEvent): void => { }
    },

    initParams: {
        placeholder: ValidatorConstants.mailPlaceholder,
        options: [Validators.required, Validators.email]
    }
}

export const passwordTITDto: TITDto = {
    inputType: 'password',
    id: '',
    formCntrlName: ValidatorConstants.passwordControlName,
    title: 'Password',
    cntrlErrMsg: '',

    events: {
        onFocus: (): void => { },
        onBlur: (): void => { }
    },

    initParams: {
        options: [Validators.required]
    }
}

function createEmptyTitDto(): TITDto
{
    return {
        inputType: '',
        id: '',
        formCntrlName: '',
        title: '',
        cntrlErrMsg: '',

        events: createEmptyTitEvents(),

        initParams: {}
    };
}

function createEmptyTitEvents(): TITEvents
{
    return {
        onFocus: (): void => { },
        onBlur: (): void => { }
    }
}