import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookIssues } from './book-issues';

describe('BookIssues', () => {
  let component: BookIssues;
  let fixture: ComponentFixture<BookIssues>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BookIssues]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BookIssues);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
