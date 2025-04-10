import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
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
import { catchError, debounceTime, Subject, takeUntil, tap } from 'rxjs';
import { ROUTER_TOKENS } from 'src/app/infrastructure/constants/routing-constants';
import {
  changePlaceholderOnBlur,
  removeValueOnFocus,
} from 'src/app/core/utilities/event-helpers';
import { passwordValidator } from 'src/app/shared/validators/user-credential-validators';
import { AuthenticationService } from '../services/user-admin.service';

@Component({
  selector: 'pm-login',
  imports: [ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit, AfterViewInit {
  private debounceTime: number = 800;

  private emailCntrl: AbstractControl<any, any> | null = null;
  private pswdCntrl: AbstractControl<any, any> | null = null;
  private rmeCntrl: AbstractControl<any, any> | null = null;

  // Create viewchild for password input and from the blur event get an observable
  // to which you caan subscribe and debounce with some time value
  // @ViewChild('', { static: true }) email;

  readonly ROUTER_TOKENS = ROUTER_TOKENS;

  emailErrMsg: string = '';
  pswdErrMsg: string = '';
  loginErrMsg: string = '';
  rmeCntrlName: string = ValidatorConstants.rememberMeControlName;

  loginFormGroup: FormGroup = this.formBuilder.group({
    email: [
      ValidatorConstants.mailPlaceholder,
      [Validators.required, Validators.email],
    ],
    password: [
      '',
      Validators.required,
      Validators.maxLength(24),
      Validators.minLength(8),
      passwordValidator(ValidatorConstants.passwordRegex),
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
        tap(
          () =>
            (this.emailErrMsg = this.validationService.contCustValErrorToString(
              this.loginFormGroup,
              ValidatorConstants.emailControlName
            ))
        )
      )
      .subscribe();

    this.pswdCntrl?.valueChanges
      .pipe(
        takeUntil(this.unsub),
        debounceTime(this.debounceTime),
        tap(
          () =>
            (this.pswdErrMsg = this.validationService.contCustValErrorToString(
              this.loginFormGroup,
              ValidatorConstants.passwordControlName
            ))
        )
      )
      .subscribe();
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

  pswdBlur(): void {
    this.pswdErrMsg = this.validationService.contCustValErrorToString(
      this.loginFormGroup,
      ValidatorConstants.passwordControlName
    );
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
        catchError((err: string) => (this.loginErrMsg = err))
      )
      .subscribe();
  }
}
