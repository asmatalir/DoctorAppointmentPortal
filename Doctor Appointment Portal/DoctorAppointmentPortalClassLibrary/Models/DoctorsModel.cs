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
        public int ExperienceYears { get; set; }
        public decimal ConsultationFees { get; set; }
        public string Description { get; set; }
        public string HospitalName { get; set; }
        public string Address { get; set; }
        public decimal Rating { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        public List<DoctorsModel> DoctorsList { get; set; }
        public List<SpecializationsModel> SpecializationsList { get; set; }
    }
}
