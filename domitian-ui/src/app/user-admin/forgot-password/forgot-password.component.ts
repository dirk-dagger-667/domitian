import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RegLogHeaderWidgetComponent } from '../wrappers/reg-log-header-widget/reg-log-header-widget.component';
import { CommonModule } from '@angular/common';
import { ValidationService } from '../services/validation.service';
import { ValidatorConstants } from 'src/app/infrastructure/constants/validation-constants';
import { UserAdminBase } from '../reg-log-base';
import { Router } from '@angular/router';
import { debounceTime } from 'rxjs/internal/operators/debounceTime';
import { TextInputTitledComponent } from "../../shared/components/text-input-titled/text-input-titled.component";
import { TITDto } from 'src/app/shared/contracts/titdto';

@Component({
    selector: 'app-forgot-password',
    templateUrl: './forgot-password.component.html',
    styleUrls: ['./forgot-password.component.css'],
    imports: [ReactiveFormsModule, RegLogHeaderWidgetComponent, CommonModule, TextInputTitledComponent]
})
export class ForgotPasswordComponent extends UserAdminBase implements OnInit {

  private emailCntrl: AbstractControl<any, any> | null = null;

  public debounceTime: number = 1000;

  public email: string = '';
  public emailControlName: string = ValidatorConstants.emailControlName;

  // public emailCntrlInput: TITDto = {
  //   inputType: 'text',
  //   formCntrlName: this.emailControlName,
  //   title: 'Email',
  //   cntrlErrMsg: '',
  //   isInvalid: this.isControlInvalid(this.emailControlName),

  //   formCntrl: this.emailCntrl as FormControl,

  //   onFocus: ($event: FocusEvent): void => { },
  //   onBlur: ($event: FocusEvent): void => { }
  // }

  constructor(private readonly validationService: ValidationService,
    private readonly router: Router,
    private formBuilder: FormBuilder) 
    { super(); }

  forgotPasswordGroup: FormGroup = this.formBuilder.group({
    email: [ValidatorConstants.mailPlaceholder, [Validators.required, Validators.email]]
  });

  ngOnInit(): void
  {
    this.emailCntrl = this.forgotPasswordGroup.get(this.emailControlName);

    this.emailCntrl?.valueChanges.pipe(debounceTime(this.debounceTime))
      .pipe(debounceTime(this.debounceTime))
      .subscribe((value: any) => this.email = value);
  }

  isControlInvalid(controlName: string)
  {
    return this.validationService.isControlInvalid(this.forgotPasswordGroup, controlName);
  }

  onSubmit()
  {
    this.router.navigate(['/', 'change-password-confirmation']);
  }

}
