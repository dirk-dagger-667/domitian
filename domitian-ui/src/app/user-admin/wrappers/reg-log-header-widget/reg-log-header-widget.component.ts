import { CommonModule } from '@angular/common';
import { Component, Input, TemplateRef } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'dominitian-reg-log-header-widget',
  templateUrl: './reg-log-header-widget.component.html',
  styleUrls: ['./reg-log-header-widget.component.css'],
  standalone: true,
  imports: [CommonModule, RouterLink]
})
export class RegLogHeaderWidgetComponent {

  @Input() regLogCustomContent!: TemplateRef<any>;
  @Input() pageTitle: string = 'Page Title was not provided!!!';
  @Input() slogan: string = 'Slogan was not provided!!!';
}
