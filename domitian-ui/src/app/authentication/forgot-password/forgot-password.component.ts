import { AfterViewInit, Component, OnInit } from '@angular/core';
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
import { debounceTime } from 'rxjs/internal/operators/debounceTime';
import { Subject, takeUntil, tap } from 'rxjs';
import { ChangePlaceholderOnBlurFocusDirective } from 'src/app/shared/directives/chnage-placeholder-on-blur/change-placeholder-on-blur-focus.directive';
import { ForgotPasswordService } from './services/forgot-password.service';

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
export class ForgotPasswordComponent implements OnInit, AfterViewInit {
  private emailCntrl: AbstractControl<any, any> | null = null;
  private readonly unsub: Subject<void> = new Subject();

  public debounceTime: number = 800;
  public placeholder = ValidatorConstants.mailPlaceholder;

  public forgotPasswordGroup: FormGroup = this.formBuilder.group({
    email: [
      ValidatorConstants.mailPlaceholder,
      [Validators.required, Validators.email],
    ],
  });

  constructor(
    private readonly validationService: ValidationService,
    readonly forgotPasswordService: ForgotPasswordService,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit(): void {
    this.emailCntrl = this.forgotPasswordGroup.get(
      ValidatorConstants.emailControlName
    );
  }

  ngAfterViewInit(): void {
    this.emailCntrl?.valueChanges
      .pipe(
        takeUntil(this.unsub),
        debounceTime(this.debounceTime),
        tap((email: string) => {
          this.forgotPasswordService.setEmail(email);

          this.forgotPasswordService.setEmailError(
            this.validationService.contCustValErrorToString(
              this.forgotPasswordGroup,
              ValidatorConstants.emailControlName
            )
          );
        })
      )
      .subscribe();
  }

  onSubmit() {
    this.forgotPasswordService.resetPassword();
  }
}
