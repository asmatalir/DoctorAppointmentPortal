import { TestBed } from '@angular/core/testing';

import { AddEditService } from './add-edit-service';

describe('AddEditService', () => {
  let service: AddEditService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AddEditService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
