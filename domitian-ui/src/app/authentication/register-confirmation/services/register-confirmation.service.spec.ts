import { TestBed } from '@angular/core/testing';

import { RegisterConfirmationService } from './register-confirmation.service';

describe('RegisterConfirmationService', () => {
  let service: RegisterConfirmationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RegisterConfirmationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
