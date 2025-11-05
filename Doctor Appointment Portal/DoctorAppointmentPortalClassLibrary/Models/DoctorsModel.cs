using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentPortalClassLibrary.Models
{
    public class DoctorsModel
    {
        public int DoctorId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }


        [Required]
        public string Gender { get; set; }

        [Required]
        [EmailAddress]
        public string DoctorEmail { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 10)]
        [RegularExpression(@"^[0-9]*$")]
        public string ContactNumber { get; set; }

        [Required]
        public string UserName { get; set; }

        public string HashedPassword { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public int ExperienceYears { get; set; }

        [Required]
        public decimal ConsultationFees { get; set; }
        public string Description { get; set; }

        [Required]
        public string HospitalName { get; set; }
        public decimal Rating { get; set; }
        public int TotalRecords { get; set; }

        public string Address { get; set; }
        public int? AddressId { get; set; }

        [Required]
        public int StateId { get; set; }

        [Required]
        public int DistrictId { get; set; }

        [Required]
        public int TalukaId { get; set; }

        [Required]
        public int CityId { get; set; }

        [Required]
        public string Pincode { get; set; }

        public string AddressLine { get; set; }

        public string SearchedDoctorName { get; set; }
        public int? SelectedSpecializationId { get; set; }
        public int? SelectedCityId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public string QualificationIds { get; set; }
        public string QualificationNames { get; set; }
        public string SpecializationNames { get; set; }
        public string SpecializationIds { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
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
