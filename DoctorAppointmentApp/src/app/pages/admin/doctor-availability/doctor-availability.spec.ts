import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DoctorAvailability } from './doctor-availability';

describe('DoctorAvailability', () => {
  let component: DoctorAvailability;
  let fixture: ComponentFixture<DoctorAvailability>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DoctorAvailability]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DoctorAvailability);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
