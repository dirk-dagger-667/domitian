import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, OnInit, signal } from '@angular/core';
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
  MustMatch,
  passwordValidator,
} from 'src/app/shared/validators/user-credential-validators';
import { Router, RouterModule } from '@angular/router';
import { ROUTER_TOKENS } from 'src/app/infrastructure/constants/routing-constants';
import { catchError, debounceTime, Subject, takeUntil, tap } from 'rxjs';
import { InfoSharingService } from 'src/app/core/services/info-sharing/info-sharing.service';
import { AuthenticationService } from '../services/user-admin.service';
import { ChangePlaceholderOnBlurFocusDirective } from 'src/app/shared/directives/chnage-placeholder-on-blur/change-placeholder-on-blur-focus.directive';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css'],
  imports: [
    ReactiveFormsModule,
    CommonModule,
    RouterModule,
    ChangePlaceholderOnBlurFocusDirective,
  ],
})
export class RegistrationComponent implements AfterViewInit, OnInit {
  private debounceTime: number = 800;

  private emailCntrl: AbstractControl<any, any> | null = null;
  private pswdCntrl: AbstractControl<any, any> | null = null;
  private cnfrmPswdCntrl: AbstractControl<any, any> | null = null;

  private readonly unsub: Subject<void> = new Subject();

  public emailErrMsg: string = '';
  public pswdErrMsg: string = '';
  public cnfrmPswdErrMsg: string = '';
  public cnfrmRspnErrMsg: string = '';

  public placeholder = ValidatorConstants.mailPlaceholder;

  public registrationFormGroup: FormGroup = this.formBuilder.group(
    {
      email: [
        ValidatorConstants.mailPlaceholder,
        [Validators.required, Validators.email],
      ],
      password: [
        '',
        [
          Validators.required,
          Validators.maxLength(24),
          Validators.minLength(8),
          passwordValidator(ValidatorConstants.passwordRegex),
        ],
      ],
      confirmPassword: [''],
    },
    {
      validators: MustMatch(
        ValidatorConstants.passwordControlName,
        ValidatorConstants.confirmPasswordControlName
      ),
    }
  );

  constructor(
    private readonly validationService: ValidationService,
    private readonly authenticationService: AuthenticationService,
    private readonly router: Router,
    private readonly formBuilder: FormBuilder,
    private readonly dataSharingService: InfoSharingService
  ) {}

  ngOnInit(): void {
    this.emailCntrl = this.registrationFormGroup.get(
      ValidatorConstants.emailControlName
    );

    this.pswdCntrl = this.registrationFormGroup.get(
      ValidatorConstants.passwordControlName
    );

    this.cnfrmPswdCntrl = this.registrationFormGroup.get(
      ValidatorConstants.confirmPasswordControlName
    );
  }

  ngAfterViewInit(): void {
    this.emailCntrl?.valueChanges
      .pipe(
        takeUntil(this.unsub),
        debounceTime(this.debounceTime),
        tap(() => {
          this.emailErrMsg = this.validationService.contCustValErrorToString(
            this.registrationFormGroup,
            ValidatorConstants.emailControlName
          );
        })
      )
      .subscribe()!;

    this.pswdCntrl?.valueChanges
      .pipe(
        takeUntil(this.unsub),
        debounceTime(this.debounceTime),
        tap(() => {
          this.pswdErrMsg = this.validationService.contCustValErrorToString(
            this.registrationFormGroup,
            ValidatorConstants.passwordControlName
          );
        })
      )
      .subscribe()!;

    this.cnfrmPswdCntrl?.valueChanges
      .pipe(
        takeUntil(this.unsub),
        debounceTime(this.debounceTime),
        tap(() => {
          this.cnfrmPswdErrMsg =
            this.validationService.contCustValErrorToString(
              this.registrationFormGroup,
              ValidatorConstants.confirmPasswordControlName
            );
        })
      )
      .subscribe()!;
  }

  onPswdBlur($event: FocusEvent): void {
    setTimeout(() => {
      const htmlTarget = $event.target as HTMLElement;
      if (
        htmlTarget.getAttribute('formControlName') ===
        ValidatorConstants.passwordControlName
      ) {
        this.pswdErrMsg = this.validationService.contCustValErrorToString(
          this.registrationFormGroup,
          ValidatorConstants.passwordControlName
        );
      } else {
        this.cnfrmPswdErrMsg = this.validationService.contCustValErrorToString(
          this.registrationFormGroup,
          ValidatorConstants.confirmPasswordControlName
        );
      }
    }, this.debounceTime);
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
