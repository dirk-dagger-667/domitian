import { TestBed } from '@angular/core/testing';

import { InfoSharingService } from './info-sharing.service';

describe('InfoSharingService', () => {
  let service: InfoSharingService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(InfoSharingService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
