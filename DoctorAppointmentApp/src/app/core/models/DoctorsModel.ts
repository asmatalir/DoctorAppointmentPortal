import { CitiesModel } from "./CitiesModel";
import { DistrictsModel } from "./DistrictsModel";
import { DoctorAvailabilitiesModel } from "./DoctorAvailabilitiesModel";
import { DoctorUnavailabilityModel } from "./DoctorUnavailablityModel";
import { QualificationsModel } from "./QualificationsModel";
import { SpecializationsModel } from "./SpecializationsModel";
import { StatesModel } from "./StatesModel";
import { TalukasModel } from "./TalukasModel";

export class DoctorsModel {
    // Search & filtering
    DoctorNameSearch?: string = '';
    SelectedHospital?: string = '';
    PageNumber: number = 1;
    PageSize: number = 5;
    SortColumn?: string = '';
    SortOrder?: string = '';
    SelectedIsActive?: number = -1;
    // SelectedDoctors?: DoctorItem[];

    // Doctor properties
    DoctorId: number = 0;
    FirstName?: string = '';
    LastName?: string = '';
    SearchedDoctorName : string;
    Gender?: string = '';
    ExperienceYears: number | null = null;
    ConsultationFees: number | null = null;
    DateOfBirth: string = '';            
    ContactNumber?: string = '';          
    DoctorEmail?: string = '';                  
    UserName?: string = '';               
    HashedPassword?: string = '';  
    Specializations: any[] = [];          
    Qualifications: any[] = [];  
    Description?: string = '';
    HospitalName?: string = '';
    Specialization : string = '';
    SelectedSpecializationId : number | null = null;
    SpecializationId : number;
    SpecializationIds: string = '';  
    QualificationIds: string = ''; 
    Rating : number;

     // Address Information
     StateId?: number | null = null;
     DistrictId?: number | null = null;
     TalukaId?: number | null = null;
     CityId?: number | null = null;
     SelectedCityId?: number | null = null;
     AddressLine?: string = '';
     Pincode?: string = '';
 
     IsActive: boolean = false;
     CreatedBy: number | null = null;
     AddressId: number | null = null;
     Address?: string = '';

     SpecializationsList : SpecializationsModel[];
     SpecializationNames : string ;
     QualificationNames : string ;
     QualificationsList : QualificationsModel[];
     StatesList : StatesModel[];
     DistrictsList : DistrictsModel[];
     TalukasList : TalukasModel[];
     CitiesList : CitiesModel[];
     DoctorAvailabilityList : DoctorAvailabilitiesModel[] = [];
     DoctorAvailabilityExceptionsList : DoctorUnavailabilityModel[] = [];

}