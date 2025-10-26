import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

// Import the modal component
import { DoctorAvailableSlotsModal } from '../pages/Patient/doctor-available-slots-modal/doctor-available-slots-modal';
import { Homepage } from './homepage/homepage';

@NgModule({
  declarations: [
    DoctorAvailableSlotsModal,
    Homepage  
  ],
  imports: [
    CommonModule,
    FormsModule,
    NgbModule
  ],
  exports: [
    DoctorAvailableSlotsModal 
  ]
})
export class SharedModule {}
