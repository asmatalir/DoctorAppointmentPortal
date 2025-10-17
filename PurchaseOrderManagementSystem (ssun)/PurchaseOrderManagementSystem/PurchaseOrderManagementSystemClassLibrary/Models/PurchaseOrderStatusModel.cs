using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseOrderManagementSystemClassLibrary.Models
{
    public class PurchaseOrderStatusModel
    {
        public int PurchaseOrderStatusId { get; set; }
        public string PurchaseOrderStatus { get; set; }
        public string ShortCode { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        public List<PurchaseOrderStatusModel> PurchaseOrderStatusList { get; set; }
    }
}
