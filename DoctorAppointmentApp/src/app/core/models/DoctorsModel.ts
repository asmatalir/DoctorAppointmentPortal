export class DoctorsModel {
    // Search & filtering
    DoctorNameSearch?: string = '';
    SelectedHospital?: string = '';
    PageNo: number = 1;
    PageSize: number = 5;
    SortColumn?: string = '';
    SortOrder?: string = '';
    SelectedIsActive?: number = -1;
    // SelectedDoctors?: DoctorItem[];

    // Doctor properties
    DoctorId: number = 0;
    FirstName?: string = '';
    LastName?: string = '';
    Gender?: string = '';
    ExperienceYears: number | null = null;
    ConsultationFees: number | null = null;
    Description?: string = '';
    HospitalName?: string = '';
    AddressId: number | null = null;
    FullAddress?: string = '';
    Rating: number | null = null;
    IsActive: boolean = false;
    CreatedBy: number | null = null;
}