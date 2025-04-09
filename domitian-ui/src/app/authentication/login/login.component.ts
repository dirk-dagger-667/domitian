import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  ReactiveFormsModule,
  AbstractControl,
} from '@angular/forms';
import { ValidationService } from '../services/validation.service';
import { ValidatorConstants } from 'src/app/infrastructure/constants/validation-constants';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { debounceTime } from 'rxjs';
import { UserAdminBase } from '../reg-log-base';
import { ROUTER_TOKENS } from 'src/app/infrastructure/constants/routing-constants';
import { UserAdminService } from '../services/user-admin.service';
import { TITDto, updateTITDto } from 'src/app/shared/contracts/titdto';
import { TextInputTitledComponent } from 'src/app/shared/components/text-input-titled/text-input-titled.component';
import {
  emailTITDto,
  passwordTITDto,
} from 'src/app/core/factories/object-factories';
import {
  changePlaceholderOnBlur,
  removeValueOnFocus,
} from 'src/app/core/utilities/event-helpers';

@Component({
  selector: 'pm-login',
  imports: [
    ReactiveFormsModule,
    CommonModule,
    RouterLink,
    TextInputTitledComponent,
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent
  extends UserAdminBase
  implements OnInit, AfterViewInit
{
  private debounceTime: number = 800;

  public emailCntrl: AbstractControl<any, any> | null = null;
  private pswdCntrl: AbstractControl<any, any> | null = null;
  private rmeCntrl: AbstractControl<any, any> | null = null;

  readonly ROUTER_TOKENS = ROUTER_TOKENS;

  emailErrMsg: string = '';
  pswdErrMsg: string = '';
  loginErrMsg: string = '';
  rmeCntrlName: string = ValidatorConstants.rememberMeControlName;

  emailCntrlInput: TITDto;
  pswdCntrlInput: TITDto;

  loginFormGroup: FormGroup = this.formBuilder.group({
    rememberMe: [false],
  });

  constructor(
    private readonly validationService: ValidationService,
    private readonly userAdminService: UserAdminService,
    private readonly formBuilder: FormBuilder,
    private readonly router: Router,
    private readonly activatedRoute: ActivatedRoute
  ) {
    super();

    this.emailCntrlInput = emailTITDto;
    this.emailCntrlInput = updateTITDto(this.emailCntrlInput, {
      events: {
        onFocus: ($event: FocusEvent): void =>
          removeValueOnFocus($event, ValidatorConstants.mailPlaceholder),
        onBlur: ($event: FocusEvent): void =>
          changePlaceholderOnBlur(
            $event,
            this.emailCntrl,
            ValidatorConstants.mailPlaceholder
          ),
      },
    });

    this.pswdCntrlInput = passwordTITDto;
  }

  ngOnInit(): void {
    this.rmeCntrl = this.loginFormGroup.get(this.rmeCntrlName);
  }

  ngAfterViewInit(): void {
    this.emailCntrl = this.loginFormGroup.get(
      ValidatorConstants.emailControlName
    );

    this.emailCntrl?.valueChanges
      .pipe(debounceTime(this.debounceTime))
      .subscribe(
        () => (this.emailCntrlInput = this.updateErrMsg(this.emailCntrlInput))
      );

    this.pswdCntrl = this.loginFormGroup.get(
      ValidatorConstants.passwordControlName
    );

    this.pswdCntrl?.valueChanges
      .pipe(debounceTime(this.debounceTime))
      .subscribe(
        () => (this.pswdCntrlInput = this.updateErrMsg(this.pswdCntrlInput))
      );
  }

  private updateErrMsg(cntrlInput: TITDto): TITDto {
    let errMsg = this.validationService.contCustValErrorToString(
      this.loginFormGroup,
      cntrlInput.formCntrlName
    );

    return updateTITDto(cntrlInput, { cntrlErrMsg: errMsg });
  }

  onSubmit(): void {
    let email = this.emailCntrl?.value;
    let pswd = this.pswdCntrl?.value;
    let rme = this.rmeCntrl?.value;

    this.subs.push(
      this.userAdminService
        .login({ Email: email, Password: pswd, RememberMe: rme })
        .subscribe({
          next: () => {
            this.router.navigate([
              '../',
              `${ROUTER_TOKENS.DASHBOARD}`,
              {
                relativeTo: this.activatedRoute,
              },
            ]);
          },
          error: (err: string) => {
            this.loginErrMsg = err;
          },
        })
    );
  }
}
