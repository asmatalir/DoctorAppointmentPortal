using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentPortalClassLibrary.Models
{
    public class AppointmentRequestsModel
    {
        public int AppointmentRequestId { get; set; }

        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int StatusId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PatientEmail { get; set; }
        public string ContactNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string MedicalHistory { get; set; }
        public string InsuranceInfo { get; set; }

        public string DoctorEmail { get; set; }
        public int SpecializationId { get; set; }
        public int SelectedSpecializationId { get; set; }
        public string MedicalConcern { get; set; }
        public DateTime PreferredDate { get; set; }
        public int SlotId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan FinalStartTime { get; set; }
        public TimeSpan FinalEndTime { get; set; }
        public DateTime FinalDate { get; set; }

        public DateTime CreatedOn { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }

        public string SpecializationName { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string StatusName { get; set; }

        public int StateId { get; set; }
        public int DistrictId { get; set; }
        public int TalukaId { get; set; }
        public int CityId { get; set; }
        public string AddressLine { get; set; }
        public string Pincode { get; set; }
        public int AddressId { get; set; }
        public string Address { get; set; }

        public string Action { get; set; }
        public string SearchedPatientName { get; set; }
        public string SearchedDoctorName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }

        public int CreatedBy { get; set; }
        public List<AppointmentRequestsModel> AppointmentRequestList { get; set; }
        public List<SpecializationsModel> SpecializationsList { get; set; }
        public List<StatusesModel> StatusesList { get; set; }

        public List<StatesModel> StatesList { get; set; }
        public List<DistrictsModel> DistrictsList { get; set; }
        public List<TalukasModel> TalukasList { get; set; }
        public List<CitiesModel> CitiesList { get; set; }
    }
}
