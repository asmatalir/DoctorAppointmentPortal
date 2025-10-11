using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentPortalClassLibrary.Models
{
    public class CitiesModel
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int TalukaId { get; set; }
        public int DistrictId { get; set; }
        public int StateId { get; set; }

        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        public List<CitiesModel> CitiesList { get; set; }
    }
}
