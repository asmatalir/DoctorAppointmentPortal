import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DoctorAddedit } from './doctor-addedit';

describe('DoctorAddedit', () => {
  let component: DoctorAddedit;
  let fixture: ComponentFixture<DoctorAddedit>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DoctorAddedit]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DoctorAddedit);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
