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
import {
  changePlaceholderOnBlur,
  removeValueOnFocus,
} from 'src/app/core/utilities/event-helpers';
import { Subject, takeUntil, tap } from 'rxjs';
import { AuthenticationService } from '../services/user-admin.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css'],
  imports: [ReactiveFormsModule, CommonModule],
})
export class ForgotPasswordComponent implements OnInit {
  private emailCntrl: AbstractControl<any, any> | null = null;

  public debounceTime: number = 1000;

  public email: string = '';
  public emailControlName: string = ValidatorConstants.emailControlName;

  public emailErrMsg: string = '';

  public forgotPasswordGroup: FormGroup = this.formBuilder.group({
    email: [
      ValidatorConstants.mailPlaceholder,
      [Validators.required, Validators.email],
    ],
  });

  private readonly unsub: Subject<void> = new Subject();

  constructor(
    private readonly validationService: ValidationService,
    private readonly authenticationService: AuthenticationService,
    private readonly router: Router,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit(): void {
    this.emailCntrl = this.forgotPasswordGroup.get(this.emailControlName);

    this.emailCntrl?.valueChanges
      .pipe(
        takeUntil(this.unsub),
        debounceTime(this.debounceTime),
        tap((value: string) => (this.email = value))
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

  onSubmit() {
    //Send an email message to the specified email with the confirmation of the password

    // this.authenticationService.

    this.router.navigate(['/', 'change-password-confirmation']);
  }
}
