import { Component } from '@angular/core';
import { RegLogHeaderWidgetComponent } from '../wrappers/reg-log-header-widget/reg-log-header-widget.component';

@Component({
  selector: 'app-change-password-confirmation',
  templateUrl: './change-password-confirmation.component.html',
  styleUrls: ['./change-password-confirmation.component.css'],
  standalone: true,
  imports: [RegLogHeaderWidgetComponent]
})
export class ChangePasswordConfirmationComponent {

}
