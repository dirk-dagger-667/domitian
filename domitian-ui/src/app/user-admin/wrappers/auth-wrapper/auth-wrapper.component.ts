import { CommonModule } from '@angular/common';
import { Component, Input, TemplateRef } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'dominitian-auth-wrapper',
  templateUrl: './auth-wrapper.component.html',
  styleUrls: ['./auth-wrapper.component.css'],
  imports: [CommonModule, RouterLink],
})
export class AuthWrapperComponent {
  @Input() authTemplateContent!: TemplateRef<any>;
  @Input() pageTitle: string = 'Page Title was not provided!!!';
  @Input() slogan: string = 'Slogan was not provided!!!';
}
