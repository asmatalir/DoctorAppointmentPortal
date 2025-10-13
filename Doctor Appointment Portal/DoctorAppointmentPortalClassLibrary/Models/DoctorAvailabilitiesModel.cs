using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentPortalClassLibrary.Models
{
    public class DoctorAvailabilitiesModel
    {
        public int DayOfWeek { get; set; }          
        public TimeSpan StartTime { get; set; }     
        public TimeSpan EndTime { get; set; }
        public int Duration { get; set; }           
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }          
        public DateTime CreatedOn { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
    }
}
