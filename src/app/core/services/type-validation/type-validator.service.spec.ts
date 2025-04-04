import { TestBed } from '@angular/core/testing';

import { TypeValidatorService } from './type-validator.service';

describe('TypeValidatorService', () => {
  let service: TypeValidatorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TypeValidatorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
