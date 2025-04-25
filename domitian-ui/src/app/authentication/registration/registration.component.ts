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
import { RouterModule } from '@angular/router';
import { debounceTime, Subject, takeUntil, tap } from 'rxjs';
import { ChangePlaceholderOnBlurFocusDirective } from 'src/app/shared/directives/chnage-placeholder-on-blur/change-placeholder-on-blur-focus.directive';
import { RegistrationService } from './services/registration.service';

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
    private readonly formBuilder: FormBuilder,
    readonly registrationService: RegistrationService
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
        tap((email: string) => {
          this.registrationService.setEmail(email);

          this.registrationService.setEmailError(
            this.validationService.contCustValErrorToString(
              this.registrationFormGroup,
              ValidatorConstants.emailControlName
            )
          );
        })
      )
      .subscribe()!;

    this.pswdCntrl?.valueChanges
      .pipe(
        takeUntil(this.unsub),
        debounceTime(this.debounceTime),
        tap((password: string) => {
          this.registrationService.setPassword(password);

          this.registrationService.setPasswordError(
            this.validationService.contCustValErrorToString(
              this.registrationFormGroup,
              ValidatorConstants.passwordControlName
            )
          );
        })
      )
      .subscribe()!;

    this.cnfrmPswdCntrl?.valueChanges
      .pipe(
        takeUntil(this.unsub),
        debounceTime(this.debounceTime),
        tap((confPassword: string) => {
          this.registrationService.setConfPassword(confPassword);

          this.registrationService.setConfPassError(
            this.validationService.contCustValErrorToString(
              this.registrationFormGroup,
              ValidatorConstants.confirmPasswordControlName
            )
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
        this.registrationService.setPasswordError(
          this.validationService.contCustValErrorToString(
            this.registrationFormGroup,
            ValidatorConstants.passwordControlName
          )
        );
      } else {
        this.registrationService.setConfPassError(
          this.validationService.contCustValErrorToString(
            this.registrationFormGroup,
            ValidatorConstants.confirmPasswordControlName
          )
        );
      }
    }, this.debounceTime);
  }

  onSubmit(): void {
    this.registrationService.register();
  }
}
