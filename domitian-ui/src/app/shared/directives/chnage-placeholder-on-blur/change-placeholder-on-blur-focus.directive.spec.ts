import { inject } from '@angular/core';
import { ChangePlaceholderOnBlurFocusDirective } from './change-placeholder-on-blur-focus.directive';
import { NgControl } from '@angular/forms';

describe('ChangePlaceholderOnBlurDirective', () => {
  it('should create an instance', () => {
    const directive = new ChangePlaceholderOnBlurFocusDirective(
      inject(NgControl)
    );
    expect(directive).toBeTruthy();
  });
});
