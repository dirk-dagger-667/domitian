import { ValidatorFn, FormControlOptions } from '@angular/forms';

export interface TITDto {
  inputType: string;
  id?: string;
  formCntrlName: string;
  title: string;
  cntrlErrMsg: string;

  events: TITEvents;
  initParams: TITCntrlInitParams;
}

export interface TITEvents {
  onFocus($event: FocusEvent): void;
  onBlur($event: FocusEvent): void;
}

export interface TITCntrlInitParams {
  placeholder?: string;
  options?: ValidatorFn | ValidatorFn[] | FormControlOptions | null;
}

export function updateTITDto(obj: TITDto, changes: Partial<TITDto>): TITDto {
  return {
    ...obj,
    ...changes,
    events: {
      ...obj.events,
      ...changes.events,
    },
    initParams: {
      ...obj.initParams,
      ...changes.initParams,
    },
  };
}
