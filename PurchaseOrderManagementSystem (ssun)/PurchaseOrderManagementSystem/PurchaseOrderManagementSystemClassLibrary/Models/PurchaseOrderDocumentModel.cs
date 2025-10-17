using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseOrderManagementSystemClassLibrary.Models
{
    public class PurchaseOrderDocumentModel
    {
        public int PurchaseOrdersDocumentId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentFileName { get; set; }
        public string Notes { get; set; }
    }
}
