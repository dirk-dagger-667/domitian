import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, OnInit, signal } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  ReactiveFormsModule,
  AbstractControl,
  Validators,
} from '@angular/forms';
import { ValidationService } from '../services/validation.service';
import { ValidatorConstants } from 'src/app/infrastructure/constants/validation-constants';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import {
  catchError,
  debounceTime,
  Subject,
  takeUntil,
  tap,
  throwError,
} from 'rxjs';
import { ROUTER_TOKENS } from 'src/app/infrastructure/constants/routing-constants';
import { passwordValidator } from 'src/app/shared/validators/user-credential-validators';
import { AuthenticationService } from '../services/user-admin.service';
import { ChangePlaceholderOnBlurFocusDirective } from 'src/app/shared/directives/chnage-placeholder-on-blur/change-placeholder-on-blur-focus.directive';
import { HttpErrorResponse } from '@angular/common/http';

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
})
export class LoginComponent implements OnInit, AfterViewInit {
  private debounceTime: number = 800;

  private emailCntrl: AbstractControl<any, any> | null = null;
  private pswdCntrl: AbstractControl<any, any> | null = null;
  private rmeCntrl: AbstractControl<any, any> | null = null;

  public readonly ROUTER_TOKENS = ROUTER_TOKENS;

  public placeholder = ValidatorConstants.mailPlaceholder;

  public emailErrMsg: string = '';
  public pswdErrMsg: string = '';
  public loginErrMsg: string = '';
  public rmeCntrlName: string = ValidatorConstants.rememberMeControlName;

  public readonly loginFormGroup: FormGroup = this.formBuilder.group({
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
    private readonly authenticationService: AuthenticationService,
    private readonly formBuilder: FormBuilder,
    private readonly router: Router,
    private readonly activatedRoute: ActivatedRoute
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
        tap(() => {
          this.emailErrMsg = this.validationService.contCustValErrorToString(
            this.loginFormGroup,
            ValidatorConstants.emailControlName
          );
        })
      )
      .subscribe();

    this.pswdCntrl?.valueChanges
      .pipe(
        takeUntil(this.unsub),
        debounceTime(this.debounceTime),
        tap(() => {
          this.pswdErrMsg = this.validationService.contCustValErrorToString(
            this.loginFormGroup,
            ValidatorConstants.passwordControlName
          );
        })
      )
      .subscribe();
  }

  onBlur(): void {
    setTimeout(() => {
      this.pswdErrMsg = this.validationService.contCustValErrorToString(
        this.loginFormGroup,
        ValidatorConstants.passwordControlName
      );
    }, this.debounceTime);
  }

  onSubmit(): void {
    this.authenticationService
      .login({
        Email: this.emailCntrl?.value,
        Password: this.pswdCntrl?.value,
        RememberMe: this.rmeCntrl?.value,
      })
      .pipe(
        takeUntil(this.unsub),
        tap(() =>
          this.router.navigate([
            '../',
            ROUTER_TOKENS.DASHBOARD,
            {
              relativeTo: this.activatedRoute,
            },
          ])
        ),
        catchError((err: HttpErrorResponse) => {
          this.loginErrMsg =
            'There was an error when trying to login. Try again later.';
          return throwError(() => err);
        })
      )
      .subscribe();
  }
}
