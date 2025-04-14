import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ValidationService } from '../services/validation.service';
import { ValidatorConstants } from 'src/app/infrastructure/constants/validation-constants';
import { Router } from '@angular/router';
import { debounceTime } from 'rxjs/internal/operators/debounceTime';
import { Subject, takeUntil, tap } from 'rxjs';
import { AuthenticationService } from '../services/user-admin.service';
import { ChangePlaceholderOnBlurFocusDirective } from 'src/app/shared/directives/chnage-placeholder-on-blur/change-placeholder-on-blur-focus.directive';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css'],
  imports: [
    ReactiveFormsModule,
    CommonModule,
    ChangePlaceholderOnBlurFocusDirective,
  ],
})
export class ForgotPasswordComponent implements OnInit {
  private emailCntrl: AbstractControl<any, any> | null = null;
  private readonly unsub: Subject<void> = new Subject();

  public debounceTime: number = 1000;
  public emailErrMsg: string = '';
  public placeholder = ValidatorConstants.mailPlaceholder;

  public forgotPasswordGroup: FormGroup = this.formBuilder.group({
    email: [
      ValidatorConstants.mailPlaceholder,
      [Validators.required, Validators.email],
    ],
  });

  constructor(
    private readonly validationService: ValidationService,
    private readonly authenticationService: AuthenticationService,
    private readonly router: Router,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit(): void {
    this.emailCntrl = this.forgotPasswordGroup.get(
      ValidatorConstants.emailControlName
    );

    this.emailCntrl?.valueChanges
      .pipe(
        takeUntil(this.unsub),
        debounceTime(this.debounceTime),
        tap(() => {
          this.emailErrMsg = this.validationService.contCustValErrorToString(
            this.forgotPasswordGroup,
            ValidatorConstants.passwordControlName
          );
        })
      )
      .subscribe();
  }

  onSubmit() {
    //Send an email message to the specified email with the confirmation of the password

    // this.authenticationService.

    this.router.navigate(['/', 'change-password-confirmation']);
  }
}
