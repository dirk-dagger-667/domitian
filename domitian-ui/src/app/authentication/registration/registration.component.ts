import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  Validators,
  ReactiveFormsModule,
  AbstractControl,
} from '@angular/forms';
import { ValidationService } from '../services/validation.service';
import { ValidatorConstants } from 'src/app/infrastructure/constants/validation-constants';
import {
  confirmPasswordSameValidator,
  passwordValidator,
} from 'src/app/shared/validators/user-credential-validators';
import { Router, RouterModule } from '@angular/router';
import { ROUTER_TOKENS } from 'src/app/infrastructure/constants/routing-constants';
import { catchError, debounceTime, Subject, takeUntil, tap } from 'rxjs';
import { InfoSharingService } from 'src/app/core/services/info-sharing/info-sharing.service';
import {
  changePlaceholderOnBlur,
  removeValueOnFocus,
} from 'src/app/core/utilities/event-helpers';
import { AuthenticationService } from '../services/user-admin.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css'],
  imports: [ReactiveFormsModule, CommonModule, RouterModule],
})
export class RegistrationComponent implements AfterViewInit, OnInit {
  private debounceTime: number = 1000;

  private emailCntrl: AbstractControl<any, any> | null = null;
  private pswdCntrl: AbstractControl<any, any> | null = null;
  private cnfrmPswdCntrl: AbstractControl<any, any> | null = null;

  private readonly unsub: Subject<void> = new Subject();

  constructor(
    private readonly validationService: ValidationService,
    private readonly authenticationService: AuthenticationService,
    private readonly router: Router,
    private readonly formBuilder: FormBuilder,
    private readonly dataSharingService: InfoSharingService
  ) {}

  emailErrMsg: string = '';
  pswdErrMsg: string = '';
  cnfrmPswdErrMsg: string = '';
  cnfrmRspnErrMsg: string = '';

  registrationFormGroup: FormGroup = this.formBuilder.group({
    passwordFormGroup: this.formBuilder.group(
      {
        email: [
          ValidatorConstants.mailPlaceholder,
          [Validators.required, Validators.email],
        ],
        password: [
          '',
          Validators.maxLength(24),
          Validators.minLength(8),
          passwordValidator(ValidatorConstants.passwordRegex),
        ],
        confirmPassword: [''],
      },
      { validator: confirmPasswordSameValidator }
    ),
  });

  ngOnInit(): void {
    this.emailCntrl = this.registrationFormGroup.get(
      ValidatorConstants.emailControlName
    );

    this.pswdCntrl = this.registrationFormGroup.get(
      `${ValidatorConstants.passwrodFormGroupControlName}.${ValidatorConstants.passwordControlName}`
    );

    this.cnfrmPswdCntrl = this.registrationFormGroup.get(
      `${ValidatorConstants.passwrodFormGroupControlName}.${ValidatorConstants.confirmPasswordControlName}`
    );
  }

  ngAfterViewInit(): void {
    this.emailCntrl?.valueChanges
      .pipe(
        takeUntil(this.unsub),
        debounceTime(this.debounceTime),
        tap(
          () =>
            (this.emailErrMsg = this.validationService.contCustValErrorToString(
              this.registrationFormGroup,
              ValidatorConstants.emailControlName
            ))
        )
      )
      .subscribe()!;

    this.pswdCntrl?.valueChanges
      .pipe(
        takeUntil(this.unsub),
        debounceTime(this.debounceTime),
        tap(
          () =>
            (this.pswdErrMsg = this.validationService.contCustValErrorToString(
              this.registrationFormGroup,
              ValidatorConstants.passwordControlName
            ))
        )
      )
      .subscribe()!;

    this.cnfrmPswdCntrl?.valueChanges
      .pipe(
        takeUntil(this.unsub),
        debounceTime(this.debounceTime),
        tap(
          () =>
            (this.cnfrmPswdErrMsg =
              this.validationService.contCustValErrorToString(
                this.registrationFormGroup,
                ValidatorConstants.confirmPasswordControlName
              ))
        )
      )
      .subscribe()!;
  }

  onFocus($event: FocusEvent): void {
    removeValueOnFocus($event, ValidatorConstants.mailPlaceholder);
  }

  onBlur($event: FocusEvent): void {
    changePlaceholderOnBlur(
      $event,
      this.emailCntrl,
      ValidatorConstants.mailPlaceholder
    );
  }

  onSubmit(): void {
    this.authenticationService
      .register({
        Email: this.emailCntrl?.value,
        Password: this.pswdCntrl?.value,
        ConfirmPassword: this.cnfrmPswdCntrl?.value,
      })
      .pipe(
        takeUntil(this.unsub),
        tap((resp) => {
          this.dataSharingService.sendData({
            callbackUrl: resp,
            email: this.emailCntrl?.value,
          });

          this.navigateRegister(resp);
        }),
        catchError((err: string) => (this.cnfrmRspnErrMsg = err))
      )
      .subscribe();
  }

  private navigateRegister(response: any): void {
    if (response.dominitianIsNullOrEmpty()) {
      this.router.navigate(['../', `${ROUTER_TOKENS.LOGIN}`]);
    } else {
      this.router.navigate(['../', `${ROUTER_TOKENS.REG_CONFIRMATION}`]);
    }
  }
}
