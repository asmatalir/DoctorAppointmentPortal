import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DoctorAppointmentRequests } from './doctor-appointment-requests';

describe('DoctorAppointmentRequests', () => {
  let component: DoctorAppointmentRequests;
  let fixture: ComponentFixture<DoctorAppointmentRequests>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DoctorAppointmentRequests]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DoctorAppointmentRequests);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
