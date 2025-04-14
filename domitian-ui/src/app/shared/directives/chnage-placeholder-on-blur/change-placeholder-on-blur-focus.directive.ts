import { Directive, HostListener, input } from '@angular/core';
import { NgControl } from '@angular/forms';
import { isDefined } from 'src/app/infrastructure/guards/type-guards';

@Directive({
  selector: '[appChangePlaceholderOnBlurFocus]',
})
export class ChangePlaceholderOnBlurFocusDirective {
  placeholder = input<string | null>(null, {
    alias: 'appChangePlaceholderOnBlurFocus',
  });

  constructor(private control: NgControl) {}

  @HostListener('blur') handleBlur(): void {
    this.handlePlaceholderChange();
  }

  @HostListener('focus') handleFocus(): void {
    this.handlePlaceholderChange();
  }

  private handlePlaceholderChange(): void {
    if (isDefined(this.control.control?.value)) {
      this.control.control?.setValue(
        this.control.control?.value.toLocaleLowerCase() === this.placeholder()
          ? ''
          : this.control.control?.value
      );
    }
  }
}
