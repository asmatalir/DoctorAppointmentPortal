using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseOrderManagementSystemClassLibrary.Models
{
    public class UserInfoModel
    {
        public int UserCredentialsId { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Username must be more than 4 characters")]
        public string UserName { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        [DataType(DataType.Password)]
        public string EnteredPrimaryPassword { get; set; }

        //[Required(ErrorMessage = "OTP is required.")]
        //[StringLength(10, MinimumLength = 4, ErrorMessage = "OTP must be between 4 and 10 characters.")]
        public string PrimaryPassword { get; set; }
        public string SecondaryPassword { get; set; }
        public string Email { get; set; }
        public string EnteredSecondaryPassword { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

       // public List<UserDetailsModel> UserRoleList { get; set; }
    }
}
