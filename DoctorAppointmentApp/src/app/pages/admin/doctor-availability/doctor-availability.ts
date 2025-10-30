import { Component } from '@angular/core';
import { DoctorAvailabilitiesModel } from '../../../core/models/DoctorAvailabilitiesModel';
import { DoctorSessionsModel } from '../../../core/models/DoctorSessionsModel';
import { DoctorsModel } from '../../../core/models/DoctorsModel';
import { ActivatedRoute, Router } from '@angular/router';
import { DoctorsService } from '../../../core/services/doctors-service';
import { DoctorUnavailabilityModel } from '../../../core/models/DoctorUnavailablityModel';
import { ToastService } from '../../../core/services/toast-service';

@Component({
  selector: 'app-doctor-availability',
  standalone: false,
  templateUrl: './doctor-availability.html',
  styleUrl: './doctor-availability.scss'
})
export class DoctorAvailability {

  doctor: DoctorsModel = new DoctorsModel();


  constructor(
    private doctorService: DoctorsService,
    private route: ActivatedRoute,
    private router: Router,
    private toastService: ToastService
  ) { }

  ngOnInit(): void {
    const doctorId = Number(this.route.snapshot.paramMap.get('id'));
    if (doctorId) {
      this.loadDoctorDetails(doctorId);
    }
  }

  daysOfWeek = [
    { name: 'Monday', value: 1 },
    { name: 'Tuesday', value: 2 },
    { name: 'Wednesday', value: 3 },
    { name: 'Thursday', value: 4 },
    { name: 'Friday', value: 5 },
    { name: 'Saturday', value: 6 },
    { name: 'Sunday', value: 7 }
  ];


  loadDoctorDetails(id: number) {
    this.doctorService.GetDoctorAvailabilityDetails(id).subscribe({
      next: (res: any) => {
        if (res) {
          this.doctor = res;


          this.doctor.Specializations = res.SpecializationNames?.split(',') || [];
          this.doctor.Qualifications = res.QualificationNames?.split(',') || [];


          this.doctor.DoctorAvailabilityList = res.DoctorAvailabilityList || [];
          this.doctor.DoctorAvailabilityExceptionsList = res.DoctorAvailabilityExceptionsList || [];

          this.doctor.DoctorAvailabilityExceptionsList.forEach(ex => {
            if (ex.ExceptionDate) {
              ex.ExceptionDate = new Date(ex.ExceptionDate).toISOString().split('T')[0];
            }
          });

        }
      },
      error: (err) => {
        if ((err as any).isAuthError) return;
        this.toastService.show("Error loading doctor details", { classname: 'bg-danger text-white', delay: 1500 })
      }

    });
  }


  addAvailability() {
    this.doctor.DoctorAvailabilityList.push({ DayOfWeek: '', StartTime: '', EndTime: '', Duration: 0 } as DoctorAvailabilitiesModel);
  }

  
  removeAvailability(index: number) {
    this.doctor.DoctorAvailabilityList.splice(index, 1);
  }

  validateAvailabilities(): boolean {
    const list = this.doctor.DoctorAvailabilityList;


    for (let i = 0; i < list.length; i++) {
      const a = list[i];


      if (!a.DayOfWeek || !a.StartTime || !a.EndTime || !a.Duration) {
        this.toastService.show(`Please fill all fields for slot #${i + 1}.`, { classname: 'bg-warning text-dark', delay: 2000 });
        return false;
      }


      if (a.StartTime >= a.EndTime) {
        this.toastService.show(`Start time must be before end time (slot #${i + 1}).`, { classname: 'bg-warning text-dark', delay: 2000 });
        return false;
      }


      if (a.Duration <= 0 || a.Duration > 60) {
        this.toastService.show(`Slot duration must be between 1 and 60 minutes (slot #${i + 1}).`, { classname: 'bg-warning text-dark', delay: 2000 });
        return false;
      }

      const startMinutes = this.convertToMinutes(a.StartTime);
      const endMinutes = this.convertToMinutes(a.EndTime);
      const availableMinutes = endMinutes - startMinutes;

      if (a.Duration > availableMinutes) {
        this.toastService.show(`Slot duration (${a.Duration} min) cannot be greater than available time (${availableMinutes} min) for slot #${i + 1}.`,
          { classname: 'bg-warning text-dark', delay: 2000 });
        return false;
      }
    }

    
    const days = [...new Set(list.map(a => Number(a.DayOfWeek)))];

    for (const day of days) {
      const slots = list
        .filter(a => Number(a.DayOfWeek) === day)
        .map(a => ({
          ...a,
          Start: this.convertToMinutes(a.StartTime),
          End: this.convertToMinutes(a.EndTime)
        }))
        .sort((a, b) => a.Start - b.Start);

      
      for (let i = 0; i < slots.length - 1; i++) {
        const current = slots[i];
        const next = slots[i + 1];

        if (current.End > next.Start) {
          this.toastService.show(
            `Overlapping times found for ${this.getDayName(day)} between ${current.StartTime} - ${current.EndTime} and ${next.StartTime} - ${next.EndTime}.`,
            { classname: 'bg-danger text-white', delay: 2500 }
          );
          return false;
        }
      }
    }

    return true;
  }

  
  convertToMinutes(time: string): number {
    if (!time) return 0;
    const [h, m] = time.split(':').map(Number);
    return h * 60 + m;
  }

  
  getDayName(dayValue: number): string {
    const day = this.daysOfWeek.find(d => d.value === dayValue);
    return day ? day.name : 'Unknown Day';
  }


  SaveDoctorAvailabilities() {
  if (!this.validateAvailabilities()) {
    return; 
  }
    
    this.doctorService.SaveDoctorAvailability(this.doctor).subscribe({
      next: (res) => {
        this.toastService.show("Doctor availability saved successfully", { classname: 'bg-success text-white', delay: 1500 });
         this.router.navigate(['admin/doctor-list']);

      },
      error: (err) => {
        if ((err as any).isAuthError) return;
        this.toastService.show("Error saving doctor availability", { classname: 'bg-danger text-white', delay: 1500 });
      }
    });
  }


  addUnavailability() {
    if (!this.doctor.DoctorAvailabilityExceptionsList) {
      this.doctor.DoctorAvailabilityExceptionsList = [];
    }

    this.doctor.DoctorAvailabilityExceptionsList.push({
      UnavailabilityId: 0,
      DoctorId: this.doctor.DoctorId,
      ExceptionDate: '',
      StartTime: '',
      EndTime: '',
      Reason: ''
    } as DoctorUnavailabilityModel);
  }




  removeUnavailability(index: number) {
    this.doctor.DoctorAvailabilityExceptionsList.splice(index, 1);
  }
  onCancel() {
    this.router.navigate(['admin/doctor-list']);
  }

}
