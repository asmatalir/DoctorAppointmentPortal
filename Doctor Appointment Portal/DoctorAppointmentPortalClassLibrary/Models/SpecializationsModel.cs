using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentPortalClassLibrary.Models
{
    public class SpecializationsModel
    {
        public int SpecializationId { get; set; }
        public string SpecializationName { get; set; }

        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        public List<SpecializationsModel> SpecializationsList { get; set; }
    }
}
