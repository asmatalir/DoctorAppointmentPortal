using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentPortalClassLibrary.Models
{
    public class UserProfilesModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string EnteredPassword { get; set; }
        public string FetchedPassword { get; set; }
        public string Email { get; set; }
        public int UserRoleId { get; set; }
        public string RoleName { get; set; }
        public int DoctorId { get; set; }
        public string DoctorEmail { get; set; }

        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
