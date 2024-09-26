import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RegLogHeaderWidgetComponent } from '../wrappers/reg-log-header-widget/reg-log-header-widget.component';
import { CommonModule } from '@angular/common';
import { ValidationService } from '../services/validation.service';
import { ValidatorConstants } from 'src/app/infrastructure/constants/validation-constants';
import { UserAdminBase } from '../reg-log-base';
import { Router } from '@angular/router';
import { debounceTime } from 'rxjs/internal/operators/debounceTime';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css'],
  standalone: true,
  imports: [ReactiveFormsModule, RegLogHeaderWidgetComponent, CommonModule]
})
export class ForgotPasswordComponent extends UserAdminBase implements OnInit {

  debounceTime: number = 1000;

  email: string = '';
  emailControlName: string = ValidatorConstants.emailControlName;

  constructor(private readonly validationService: ValidationService,
    private readonly router: Router,
    private formBuilder: FormBuilder) 
    { super(); }

  forgotPasswordGroup: FormGroup = this.formBuilder.group({
    email: [ValidatorConstants.mailPlaceholder, [Validators.required, Validators.email]]
  });

  ngOnInit(): void
  {
    let emailControl = this.forgotPasswordGroup.get(this.emailControlName);

    emailControl?.valueChanges.pipe(debounceTime(this.debounceTime))
      .pipe(debounceTime(this.debounceTime))
      .subscribe((value: any) => this.email = value);
  }

  isControlInvalid(controlName: string)
  {
    return this.validationService.isControlInvalid(this.forgotPasswordGroup, controlName);
  }

  onSubmit()
  {
    this.router.navigate(['/', 'change-password-confirmation']);
  }

}
