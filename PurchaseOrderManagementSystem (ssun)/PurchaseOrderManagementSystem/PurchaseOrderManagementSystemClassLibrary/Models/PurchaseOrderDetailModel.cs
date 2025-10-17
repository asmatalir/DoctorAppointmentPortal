using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseOrderManagementSystemClassLibrary.Models
{
    public class PurchaseOrderDetailModel
    {
        public PurchaseOrderModel PurchaseOrder { get; set; } =  new PurchaseOrderModel();
        public List<PurchaseOrderProductModel> Products { get; set; } = new List<PurchaseOrderProductModel>();
        public List<PurchaseOrderDocumentModel> Documents { get; set; } = new List<PurchaseOrderDocumentModel>();

        public List<ProductModel> ProductList { get; set; }
        public List<PurchaseOrderDocumentModel> DocumentsList { get; set; }
        public List<VendorContactModel> VendorContactsList { get; set; }
        public List<VendorModel> VendorsList { get; set; }
        public List<PurchaseOrderStatusModel> PurchaseOrderStatusList { get; set; }

    }
}
