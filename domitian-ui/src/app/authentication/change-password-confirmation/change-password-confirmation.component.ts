import { Component } from '@angular/core';
import { AuthWrapperComponent } from '../wrappers/auth-wrapper/auth-wrapper.component';

@Component({
  selector: 'app-change-password-confirmation',
  templateUrl: './change-password-confirmation.component.html',
  styleUrls: ['./change-password-confirmation.component.css'],
  imports: [AuthWrapperComponent],
})
export class ChangePasswordConfirmationComponent {}
