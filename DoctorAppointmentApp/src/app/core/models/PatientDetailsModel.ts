import { CitiesModel } from "./CitiesModel";
import { DistrictsModel } from "./DistrictsModel";
import { DoctorAvailabilitiesModel } from "./DoctorAvailabilitiesModel";
import { DoctorUnavailabilityModel } from "./DoctorUnavailablityModel";
import { StatesModel } from "./StatesModel";
import { TalukasModel } from "./TalukasModel";

export class PatientDetailsModel {

    DoctorId: number = 0;
    FirstName?: string = '';
    LastName?: string = '';
    Gender?: string = '';
    DateOfBirth?: string = '';            
    ContactNumber?: string = '';          
    Email?: string = '';                  
    Description?: string = '';
    InsuranceId : string;
    MedicalHistory : string;
    MedicalConcern : string;

     // Address Information
     StateId?: number | null = null;
     DistrictId?: number | null = null;
     TalukaId?: number | null = null;
     CityId?: number | null = null;
     AddressLine?: string = '';
     Pincode?: string = '';
 
     IsActive: boolean = false;
     CreatedBy: number | null = null;
     AddressId: number | null = null;
     Address?: string = '';

     StatesList : StatesModel[];
     DistrictsList : DistrictsModel[];
     TalukasList : TalukasModel[];
     CitiesList : CitiesModel[];

}