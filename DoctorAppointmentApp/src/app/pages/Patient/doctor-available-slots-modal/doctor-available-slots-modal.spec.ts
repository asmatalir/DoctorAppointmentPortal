import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DoctorAvailableSlotsModal } from './doctor-available-slots-modal';

describe('DoctorAvailableSlotsModal', () => {
  let component: DoctorAvailableSlotsModal;
  let fixture: ComponentFixture<DoctorAvailableSlotsModal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DoctorAvailableSlotsModal]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DoctorAvailableSlotsModal);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
