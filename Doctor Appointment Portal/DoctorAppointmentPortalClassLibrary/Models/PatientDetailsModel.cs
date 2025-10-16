using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentPortalClassLibrary.Models
{
    public class PatientDetailsModel
    {
        public int PatientId { get; set; } 
        public int DoctorId { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ContactNumber { get; set; } 
        public string Email { get; set; } 
        public string Description { get; set; } 

        public string InsuranceId { get; set; } 
        public string MedicalHistory { get; set; }
        public string MedicalConcern { get; set; }

        // Address Information
        public int StateId { get; set; }
        public int DistrictId { get; set; }
        public int TalukaId { get; set; }
        public int CityId { get; set; }
        public string AddressLine { get; set; }
        public string Pincode { get; set; }
        public int AddressId { get; set; }
        public string Address { get; set; }

        // Status & Metadata
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        // Dropdown / List data
        public List<StatesModel> StatesList { get; set; } 
        public List<DistrictsModel> DistrictsList { get; set; }
        public List<TalukasModel> TalukasList { get; set; }
        public List<CitiesModel> CitiesList { get; set; } 
    }
}
