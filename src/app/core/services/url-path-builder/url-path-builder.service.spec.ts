import { TestBed } from '@angular/core/testing';

import { UrlPathBuilderService } from './url-path-builder.service';

describe('UrlPathBuilderService', () => {
  let service: UrlPathBuilderService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UrlPathBuilderService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
