import { TestBed } from '@angular/core/testing';

import { LoginStoreService } from './login-store.service';

describe('LoginStoreService', () => {
  let service: LoginStoreService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LoginStoreService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
