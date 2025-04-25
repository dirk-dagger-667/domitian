import { CommonModule } from '@angular/common';
import {
  AfterViewInit,
  ChangeDetectionStrategy,
  Component,
  OnInit,
} from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  ReactiveFormsModule,
  AbstractControl,
  Validators,
} from '@angular/forms';
import { ValidationService } from '../services/validation.service';
import { ValidatorConstants } from 'src/app/infrastructure/constants/validation-constants';
import { RouterLink } from '@angular/router';
import { debounceTime, Subject, takeUntil, tap } from 'rxjs';
import { ROUTER_TOKENS } from 'src/app/infrastructure/constants/routing-constants';
import { passwordValidator } from 'src/app/shared/validators/user-credential-validators';
import { ChangePlaceholderOnBlurFocusDirective } from 'src/app/shared/directives/chnage-placeholder-on-blur/change-placeholder-on-blur-focus.directive';
import { LoginService } from './services/login.service';

@Component({
  selector: 'pm-login',
  imports: [
    ReactiveFormsModule,
    CommonModule,
    RouterLink,
    ChangePlaceholderOnBlurFocusDirective,
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoginComponent implements OnInit, AfterViewInit {
  private debounceTime: number = 800;

  private emailCntrl: AbstractControl<any, any> | null = null;
  private pswdCntrl: AbstractControl<any, any> | null = null;
  private rmeCntrl: AbstractControl<any, any> | null = null;

  readonly ROUTER_TOKENS = ROUTER_TOKENS;
  placeholder = ValidatorConstants.mailPlaceholder;
  rmeCntrlName: string = ValidatorConstants.rememberMeControlName;

  readonly loginFormGroup: FormGroup = this.formBuilder.group({
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
    rememberMe: [false],
  });

  private unsub: Subject<void> = new Subject();

  constructor(
    private readonly validationService: ValidationService,
    private readonly formBuilder: FormBuilder,
    readonly loginService: LoginService
  ) {}

  ngOnInit(): void {
    this.emailCntrl = this.loginFormGroup.get(
      ValidatorConstants.emailControlName
    );
    this.pswdCntrl = this.loginFormGroup.get(
      ValidatorConstants.passwordControlName
    );
    this.rmeCntrl = this.loginFormGroup.get(this.rmeCntrlName);
  }

  ngAfterViewInit(): void {
    this.emailCntrl?.valueChanges
      .pipe(
        takeUntil(this.unsub),
        debounceTime(this.debounceTime),
        tap((email: string) => {
          this.loginService.setEmail(email);

          this.loginService.setEmialError(
            this.validationService.contCustValErrorToString(
              this.loginFormGroup,
              ValidatorConstants.emailControlName
            )
          );
        })
      )
      .subscribe();

    this.pswdCntrl?.valueChanges
      .pipe(
        takeUntil(this.unsub),
        debounceTime(this.debounceTime),
        tap((password: string) => {
          this.loginService.setPassword(password);

          this.loginService.setPasswordError(
            this.validationService.contCustValErrorToString(
              this.loginFormGroup,
              ValidatorConstants.passwordControlName
            )
          );
        })
      )
      .subscribe();

    this.rmeCntrl?.valueChanges
      .pipe(
        takeUntil(this.unsub),
        tap((value: boolean) => this.loginService.setRembemberMe(value))
      )
      .subscribe();
  }

  onBlur(): void {
    setTimeout(() => {
      this.loginService.setPasswordError(
        this.validationService.contCustValErrorToString(
          this.loginFormGroup,
          ValidatorConstants.passwordControlName
        )
      );
    }, this.debounceTime);
  }

  onSubmit(): void {
    this.loginService.login();
  }
}
