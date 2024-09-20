import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule, AbstractControl } from '@angular/forms';
import { ValidationService } from '../services/implementations/validation.service';
import { ValidatorConstants } from 'src/app/shared/constants/validation-constants';
import { passwordValidator } from 'src/app/shared/validators/user-credential-validators';
import { RegLogHeaderWidgetComponent } from '../wrappers/reg-log-header-widget/reg-log-header-widget.component';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { IValidationService } from '../services/contracts/ivalidation.service';
import { debounceTime, tap } from 'rxjs';
import { UserAdminBase } from '../reg-log-base';
import { ROUTER_TOKENS } from 'src/app/shared/constants/routing-constants';
import { UserAdminService } from '../services/implementations/user-admin.service';
import { IUserAdminService } from '../services/contracts/iuser-admin.service';

@Component({
  selector: 'pm-login',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RegLogHeaderWidgetComponent, RouterLink],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent extends UserAdminBase implements OnInit 
{
  private debounceTime: number = 1000;

  private emailCntrl: AbstractControl<any, any> | null = null;
  private pswdCntrl: AbstractControl<any, any> | null = null;
  private rmeCntrl: AbstractControl<any, any> | null = null;

  private readonly validationService: IValidationService = inject(ValidationService);
  private readonly userAdminService: IUserAdminService = inject(UserAdminService);
  private readonly formBuilder: FormBuilder = inject(FormBuilder);
  private readonly router: Router = inject(Router);
  private readonly activatedRoute: ActivatedRoute = inject(ActivatedRoute);

  readonly ROUTER_TOKENS = ROUTER_TOKENS;

  emailErrMsg: string = '';
  pswdErrMsg: string = '';
  logAttemptErrMsg: string = '';
  emailCntrName: string = ValidatorConstants.emailControlName;
  pswdCntrName: string = ValidatorConstants.passwordControlName;
  rmeCntrlName: string = ValidatorConstants.rememberMeControlName;

  loginFormGroup: FormGroup = this.formBuilder.group({
    email: [ValidatorConstants.mailPlaceholder, [Validators.required, Validators.email]],
    password: [null, [Validators.required, Validators.maxLength(24), Validators.minLength(8), passwordValidator(ValidatorConstants.passwordRegex)]],
    rememberMe: [false]
  });

  ngOnInit(): void
  {
    this.emailCntrl = this.loginFormGroup.get(this.emailCntrName);

    this.emailCntrl?.valueChanges.pipe(debounceTime(this.debounceTime))
      .pipe(debounceTime(this.debounceTime))
      .subscribe(() =>
        this.emailErrMsg = this.validationService.contCustValErrorToString(this.loginFormGroup, this.emailCntrName));

    this.pswdCntrl = this.loginFormGroup.get(this.pswdCntrName);

    this.pswdCntrl?.valueChanges.pipe(debounceTime(this.debounceTime))
      .pipe(debounceTime(this.debounceTime))
      .subscribe(() =>
        this.pswdErrMsg = this.validationService.contCustValErrorToString(this.loginFormGroup, this.pswdCntrName));

    this.rmeCntrl = this.loginFormGroup.get(this.rmeCntrlName);
  }

  onSubmit(): void
  {
    let email = this.emailCntrl?.value;
    let pswd = this.pswdCntrl?.value;
    let rme = this.rmeCntrl?.value;

    this.subs.push(
      this.userAdminService.login({ Email: email, Password: pswd, RememberMe: rme })
        .pipe(
          tap(data =>
          {
            console.log(data);
          })
        ).subscribe({
          next: (resp) =>
          {
            var asdasd = resp;

            this.router.navigate(['../', `${ROUTER_TOKENS.DASHBOARD}`, { relativeTo: this.activatedRoute }]);
          },
          error: (error) =>
          {
            
          }
        })
    );
  }
}

