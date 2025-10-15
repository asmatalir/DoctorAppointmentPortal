export class AppointmentRequestsModel {
    AppointmentRequestId: number;
    PatientId: number;
    PatientEmail : string;
    DoctorEmail : string;
    DoctorId: number;
    StatusId: number;
    SpecializationId: number;
    MedicalConcern: string;
    PreferredDate: string;       
    StartTime: string;           
    EndTime: string;
    FinalStartTime: string;
    FinalEndTime: string;
    FinalDate: string;

    CreatedOn: string;
    LastModifiedBy: number;
    LastModifiedOn: string;

    SpecializationName: string;
    PatientName: string;
    DoctorName: string;
    StatusName: string;

    Action : string;
    SearchedPatientName : string;
    SearchedDoctorName : string;
    FromDate : string;
    ToDate : string;
    PageNumber : number=1;
    PageSize : number=5;
    TotalRecords : number;
}
