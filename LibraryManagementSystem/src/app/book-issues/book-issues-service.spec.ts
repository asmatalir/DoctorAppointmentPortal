import { TestBed } from '@angular/core/testing';

import { BookIssuesService } from './book-issues-service';

describe('BookIssuesService', () => {
  let service: BookIssuesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BookIssuesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
