using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentPortalClassLibrary.Models
{
    public class DoctorsModel
    {
        public int DoctorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string DoctorEmail { get; set; }
        public string ContactNumber { get; set; }
        public string UserName { get; set; }
        public string HashedPassword { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int ExperienceYears { get; set; }
        public decimal ConsultationFees { get; set; }
        public string Description { get; set; }
        public string HospitalName { get; set; }
        public decimal? Rating { get; set; }
        public int TotalRecords { get; set; }

        public string Address { get; set; }
        public int AddressId { get; set; }
        public int StateId { get; set; }
        public int DistrictId { get; set; }
        public int TalukaId { get; set; }
        public int? CityId { get; set; }
        public string Pincode { get; set; }
        public string AddressLine { get; set; }

        public string SearchedDoctorName { get; set; }
        public int SelectedSpecializationId { get; set; }
        public int SelectedCity { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public string QualificationIds { get; set; }
        public string QualificationNames { get; set; }
        public string SpecializationNames { get; set; }
        public string SpecializationIds { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        public List<DoctorsModel> DoctorsList { get; set; }
        public List<SpecializationsModel> SpecializationsList { get; set; }
        public List<QualificationsModel> QualificationsList { get; set; }
        public List<StatesModel> StatesList { get; set; }
        public List<DistrictsModel> DistrictsList { get; set; }
        public List<TalukasModel> TalukasList { get; set; }
        public List<CitiesModel> CitiesList { get; set; }
        public List<DoctorAvailabilitiesModel> DoctorAvailabilityList { get; set; }
        public List<DoctorAvailabilityExceptionsModel> DoctorAvailabilityExceptionsList { get; set; }


    }
}
