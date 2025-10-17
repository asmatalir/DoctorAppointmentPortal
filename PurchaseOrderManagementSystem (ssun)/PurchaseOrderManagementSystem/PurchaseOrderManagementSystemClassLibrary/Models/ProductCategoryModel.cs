using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseOrderManagementSystemClassLibrary.Models
{
    public class ProductCategoryModel
    {
        public int ProductCategoryId { get; set; }
        public string ProductCategory { get; set; }
        public string Description { get; set; }
        public string ShortCode { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        public List<ProductCategoryModel> ProductCategoryList { get; set; }
    }
}
