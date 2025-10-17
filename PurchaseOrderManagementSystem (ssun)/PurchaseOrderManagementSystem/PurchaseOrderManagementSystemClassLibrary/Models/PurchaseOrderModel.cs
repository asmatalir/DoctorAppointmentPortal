using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseOrderManagementSystemClassLibrary.Models
{
    public class PurchaseOrderModel
    {
        public int PurchaseOrderId { get; set; }
        public string PurchaseOrderSerialNumber { get; set; }
        public DateTime PurchaseOrderDate { get; set; }

        [Required(ErrorMessage = "Expected Delivery Date is required.")]
        public DateTime ExpectedDeliveryDate { get; set; }
        public DateTime ActualDeliveryDate { get; set; }

        [Required(ErrorMessage = "Vendor is required.")]
        public int VendorId { get; set; }

        [Required(ErrorMessage = "Vendor Contact is required.")]
        public int VendorContactId { get; set; }
        public string VendorName { get; set; }
        public string VendorContactName { get; set; }
        public string OrderNotes { get; set; }
        public string PaymentTerms { get; set; }
        public int InvoiceReceived { get; set; }
        public int PurchaseOrderStatusId { get; set; }
        public string PurchaseOrderStatus { get; set; }
        public decimal TotalOrderPrice { get; set; }
        public string SearchedVendorName { get; set; }
        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = 7;
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public List<int> SelectedCategories { get; set; }
        public List<int> SelectedProducts { get; set; }
        public List<int> SelectedStatus { get; set; }

        public List<ProductCategoryModel> ProductCategoryList { get; set; }
        public List<PurchaseOrderStatusModel> PurchaseOrderStatusList { get; set; }
        public List<ProductModel> ProductList { get; set; }
        public List<PurchaseOrderModel> PurchaseOrders { get; set; }

        public List<PurchaseOrderProductModel> ProductsList { get; set; }
        public List<PurchaseOrderDocumentModel> DocumentsList { get; set; }
        public List<VendorContactModel> VendorContactsList { get; set; }
        public List<VendorModel> VendorsList { get; set; }
    }

    public class PurchaseOrderProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
    }

}
