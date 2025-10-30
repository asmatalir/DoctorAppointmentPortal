import { CitiesModel } from "./CitiesModel";
import { DistrictsModel } from "./DistrictsModel";
import { StatesModel } from "./StatesModel";
import { TalukasModel } from "./TalukasModel";

export class AppointmentRequestsModel {
    AppointmentRequestId: number;
    PatientId: number;
    PatientEmail : string;
    DoctorEmail : string;
    DoctorId: number;
    StatusId: number;
    SpecializationId: number;
    SelectedSpecializationId: number;
    MedicalConcern: string;
    PreferredDate: string;       
    StartTime: string;           
    EndTime: string;
    FinalStartTime: string;
    FinalEndTime: string;
    FinalDate: string;
    AppointmentType : string = 'Upcoming';


    FirstName: string;           
    LastName: string;            
    PatientName: string;
    ContactNumber: string;        
    DateOfBirth: string;         
    Gender: string = '';            
    InsuranceInfo: string;       
    MedicalHistory: string;  
    AadhaarNumber : string;

    DoctorName: string;
    SlotId: number;  
    OldSlotId : number;

    CreatedOn: string;
    LastModifiedBy: number;
    LastModifiedOn: string;

    SpecializationName: string;

    StatusName: string;

    Action : string;
    SearchedPatientName : string;
    SearchedDoctorName : string;
    FromDate : string;
    ToDate : string;
    PageNumber : number=1;
    PageSize : number=5;
    TotalRecords : number;


         // Address Information
    StateId: number | null = null;
    DistrictId: number | null = null;
    TalukaId: number | null = null;
    CityId: number | null = null;
    AddressLine: string = '';
    Pincode: string = '';

    StatesList : StatesModel[];
    DistrictsList : DistrictsModel[];
    TalukasList : TalukasModel[];
    CitiesList : CitiesModel[];
}
