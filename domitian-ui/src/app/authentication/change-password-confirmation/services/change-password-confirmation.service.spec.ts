import { TestBed } from '@angular/core/testing';

import { ChangePasswordConfirmationService } from './change-password-confirmation.service';

describe('ChangePasswordConfirmationService', () => {
  let service: ChangePasswordConfirmationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChangePasswordConfirmationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
