import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule, AbstractControl } from '@angular/forms';
import { RegLogHeaderWidgetComponent } from '../wrappers/reg-log-header-widget/reg-log-header-widget.component';
import { ValidationService } from '../services/validation.service';
import { ValidatorConstants } from 'src/app/infrastructure/constants/validation-constants';
import { confirmPasswordSameValidator, passwordValidator } from 'src/app/shared/validators/user-credential-validators';
import { UserAdminBase } from '../reg-log-base';
import { UserAdminService } from '../services/user-admin.service';
import { Router, RouterModule } from '@angular/router';
import { ROUTER_TOKENS } from 'src/app/infrastructure/constants/routing-constants';
import { debounceTime } from 'rxjs';
import { InfoSharingService } from 'src/app/core/services/info-sharing/info-sharing.service';
import { TextInputTitledComponent } from 'src/app/shared/components/text-input-titled/text-input-titled.component';
import { TITDto, updateTITDto } from 'src/app/shared/contracts/titdto';
import { changePlaceholderOnBlur, removeValueOnFocus } from 'src/app/core/utilities/event-helpers';
import { emailTITDto, passwordTITDto } from 'src/app/core/factories/object-factories';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css'],
  imports: [ReactiveFormsModule, CommonModule, RegLogHeaderWidgetComponent, RouterModule, TextInputTitledComponent],
  standalone: true
})
export class RegistrationComponent extends UserAdminBase implements AfterViewInit
{
  private debounceTime: number = 1000;

  private emailCntrl: AbstractControl<any, any> | null = null;
  private pswdCntrl: AbstractControl<any, any> | null = null;
  private cnfrmPswdCntrl: AbstractControl<any, any> | null = null;

  constructor(
    private readonly validationService: ValidationService,
    private readonly userAdminService: UserAdminService,
    private readonly router: Router,
    private readonly formBuilder: FormBuilder,
    private readonly dataSharingService: InfoSharingService
  )
  {
    super();

    this.emailCntrlInput = emailTITDto;
    this.emailCntrlInput = updateTITDto(this.emailCntrlInput,
      {
        events: {
          onFocus: ($event: FocusEvent): void => removeValueOnFocus($event, ValidatorConstants.mailPlaceholder),
          onBlur: ($event: FocusEvent): void => changePlaceholderOnBlur($event, this.emailCntrl, ValidatorConstants.mailPlaceholder)
        }
      });

    this.pswdCntrlInput = passwordTITDto;
    this.pswdCntrlInput = updateTITDto(this.pswdCntrlInput,
      {
        containerClasses: "password-row",
        initParams: {
          options: [Validators.maxLength(24),
          Validators.minLength(8),
          passwordValidator(ValidatorConstants.passwordRegex)]
        }
      });

    this.cnfrmPswdCntrlInput = passwordTITDto;
    this.cnfrmPswdCntrlInput = updateTITDto(this.cnfrmPswdCntrlInput,
      {
        id: "confirm-password-row",
        containerClasses: "password-row",
        formCntrlName: ValidatorConstants.confirmPasswordControlName,
        title: "Confirm Password"
      });
  }

  emailCntrlInput: TITDto;
  pswdCntrlInput: TITDto;
  cnfrmPswdCntrlInput: TITDto;

  emailErrMsg: string = '';
  pswdErrMsg: string = '';
  cnfrmErrMsg: string = '';

  registrationFormGroup: FormGroup = this.formBuilder.group({
    passwordFormGroup: this.formBuilder.group({}, { validator: confirmPasswordSameValidator })
  });

  ngAfterViewInit(): void
  {
    this.emailCntrl = this.registrationFormGroup.get(ValidatorConstants.emailControlName);

    this.subs.push(this.emailCntrl?.valueChanges
      .pipe(debounceTime(this.debounceTime))
      .subscribe(() => this.emailCntrlInput = this.updateErrMsg(this.emailCntrlInput))!
    );

    this.pswdCntrl = this.registrationFormGroup.get(`${ValidatorConstants.passwrodFormGroupControlName}.${ValidatorConstants.passwordControlName}`);

    this.subs.push(this.pswdCntrl?.valueChanges
      .pipe(debounceTime(this.debounceTime))
      .subscribe(() => this.pswdCntrlInput = this.updateErrMsg(this.pswdCntrlInput))!
    );

    this.cnfrmPswdCntrl = this.registrationFormGroup.get(`${ValidatorConstants.passwrodFormGroupControlName}.${ValidatorConstants.confirmPasswordControlName}`);

    this.subs.push(this.cnfrmPswdCntrl?.valueChanges
      .pipe(debounceTime(this.debounceTime))
      .subscribe(() => this.cnfrmPswdCntrlInput = this.updateErrMsg(this.cnfrmPswdCntrlInput))!
    );
  }

  onSubmit(): void
  {
    let email = this.emailCntrl?.value;
    let pass = this.pswdCntrl?.value;
    let confPass = this.cnfrmPswdCntrl?.value;

    this.subs.push(
      this.userAdminService.register({ Email: email, Password: pass, ConfirmPassword: confPass })
        .subscribe({
          next: (resp) =>
          {
            this.dataSharingService.sendData({ callbackUrl: resp, email: email });
            this.navigateRegister(resp);
          },
          error: (error) =>
          {
            let err = error;
          }
        })
    );
  }

  private updateErrMsg(cntrlInput: TITDto): TITDto
  {
    let errMsg = this.validationService.contCustValErrorToString(this.registrationFormGroup, cntrlInput.formCntrlName)

    return updateTITDto(cntrlInput, { cntrlErrMsg: errMsg })
  }

  private navigateRegister(response: any): void
  {
    if (response.dominitianIsNullOrEmpty())
    {
      this.router.navigate(['../', `${ROUTER_TOKENS.LOGIN}`]);
    }
    else 
    {
      this.router.navigate(['../', `${ROUTER_TOKENS.REG_CONFIRMATION}`]);
    }
  }
}
