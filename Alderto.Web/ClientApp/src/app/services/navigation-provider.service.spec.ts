import { TestBed } from '@angular/core/testing';

import { GuildService } from './guild.service';

describe('GuildService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: GuildService = TestBed.get(GuildService);
    expect(service).toBeTruthy();
  });
});
