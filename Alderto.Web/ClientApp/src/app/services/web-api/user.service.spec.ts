import { TestBed } from '@angular/core/testing';

import { WebapiService } from './webapi.service';

describe('WebapiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: WebapiService = TestBed.get(WebapiService);
    expect(service).toBeTruthy();
  });
});
