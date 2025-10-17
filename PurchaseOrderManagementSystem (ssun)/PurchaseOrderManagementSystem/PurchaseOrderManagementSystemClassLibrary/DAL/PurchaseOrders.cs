using Microsoft.Practices.EnterpriseLibrary.Data;
using PurchaseOrderManagementSystemClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseOrderManagementSystemClassLibrary.DAL
{
    public class PurchaseOrders
    {
        private Database db;

        public PurchaseOrders()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        ErrorLogs errorLogsDAL = new ErrorLogs();

        public int CurrentUserId { get; set; }

        public List<PurchaseOrderModel> PurchaseOrderGetList(PurchaseOrderModel model = null)
        {
            if (model == null)
                model = new PurchaseOrderModel();

            List<PurchaseOrderModel> purchaseOrderList = new List<PurchaseOrderModel>();

            try
            {
                DbCommand com = db.GetStoredProcCommand("PurchaseOrdersGetList");

                if (!string.IsNullOrEmpty(model.SearchedVendorName))
                {
                    db.AddInParameter(com, "@VendorName", DbType.String, model.SearchedVendorName);
                }                   
                else
                {
                    db.AddInParameter(com, "@VendorName", DbType.String, DBNull.Value);
                }
                    
                if (model.SelectedCategories != null && model.SelectedCategories.Any())
                {
                    db.AddInParameter(com, "@CategoryIds", DbType.String, string.Join(",", model.SelectedCategories));
                }
                else
                {
                    db.AddInParameter(com, "@CategoryIds", DbType.String, DBNull.Value);
                }

                if (model.SelectedProducts != null && model.SelectedProducts.Any())
                {
                    db.AddInParameter(com, "@ProductIds", DbType.String, string.Join(",", model.SelectedProducts));
                }
                else
                {
                    db.AddInParameter(com, "@ProductIds", DbType.String, DBNull.Value);
                }

                if (model.SelectedStatus != null && model.SelectedStatus.Any())
                {
                    db.AddInParameter(com, "@StatusIds", DbType.String, string.Join(",", model.SelectedStatus));
                }
                else
                {
                    db.AddInParameter(com, "@StatusIds", DbType.String, DBNull.Value);
                }
                

                db.AddInParameter(com, "@PageNo", DbType.Int32,model.PageNo);
                db.AddInParameter(com, "@PageSize", DbType.Int32, model.PageSize);

                db.AddInParameter(com, "@SortColumn", DbType.String,model.SortColumn);
                db.AddInParameter(com, "@SortDirection", DbType.String,model.SortDirection);
                if (model.FromDate != null)
                {
                    db.AddInParameter(com, "@FromDate", DbType.DateTime, model.FromDate);
                }
                else
                {
                    db.AddInParameter(com, "@FromDate", DbType.DateTime, DBNull.Value);
                }
                if (model.ToDate != null)
                {
                    db.AddInParameter(com, "@ToDate", DbType.DateTime, model.ToDate);
                }
                else
                {
                    db.AddInParameter(com, "@ToDate", DbType.DateTime, DBNull.Value);
                }
                db.AddOutParameter(com, "@TotalCount", DbType.Int32, sizeof(int));

                DataSet ds = db.ExecuteDataSet(com);
                model.TotalRecords = Convert.ToInt32(db.GetParameterValue(com, "@TotalCount"));

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        PurchaseOrderModel po = new PurchaseOrderModel
                        {
                            PurchaseOrderId = Convert.ToInt32(row["PurchaseOrderId"]),
                            PurchaseOrderSerialNumber = Convert.ToString(row["PurchaseOrderSerialNumber"]),
                            PurchaseOrderDate = Convert.ToDateTime(row["PurchaseOrderDate"]),
                            ExpectedDeliveryDate = Convert.ToDateTime(row["ExpectedDeliveryDate"]),
                            VendorName = Convert.ToString(row["VendorName"]),
                            VendorContactName = Convert.ToString(row["VendorContactName"]),
                            PurchaseOrderStatus = Convert.ToString(row["PurchaseOrderStatus"]),
                            TotalOrderPrice = Convert.ToDecimal(row["TotalOrderPrice"])
                        };

                        purchaseOrderList.Add(po);
                    }
                }
            }
            catch (Exception ex)
            {
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
                Console.WriteLine($"Error in GetList: {ex.Message}");
            }

            return purchaseOrderList;
        }

        public PurchaseOrderDetailModel PurchaseOrderGetDetails(int id)
        {
            PurchaseOrderDetailModel detailModel = new PurchaseOrderDetailModel();
            try
            {
                if (id != 0)
                {
                    DbCommand com = this.db.GetStoredProcCommand("PurchaseOrderGetDetails");
                    db.AddInParameter(com, "PurchaseOrderId", DbType.Int32, id);

                    DataSet ds = this.db.ExecuteDataSet(com);

                    // Table 0: Purchase Order Summary
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];
                        detailModel.PurchaseOrder = new PurchaseOrderModel
                        {
                            PurchaseOrderSerialNumber = Convert.ToString(dt.Rows[0]["PurchaseOrderSerialNumber"]),
                            PurchaseOrderDate = Convert.ToDateTime(dt.Rows[0]["PurchaseOrderDate"]),
                            VendorName = Convert.ToString(dt.Rows[0]["VendorName"]),
                            VendorContactName = Convert.ToString(dt.Rows[0]["Name"]),
                            OrderNotes = Convert.ToString(dt.Rows[0]["OrderNotes"]),
                            ExpectedDeliveryDate = Convert.ToDateTime(dt.Rows[0]["ExpectedDeliveryDate"]),
                            PurchaseOrderStatus = Convert.ToString(dt.Rows[0]["PurchaseOrderStatus"]),
                            PaymentTerms = Convert.ToString(dt.Rows[0]["PaymentTerms"]),
                            InvoiceReceived = Convert.ToInt32(dt.Rows[0]["InvoiceReceived"]),
                            CreatedBy = Convert.ToInt32(dt.Rows[0]["CreatedBy"]),
                            CreatedOn = Convert.ToDateTime(dt.Rows[0]["CreatedOn"]),
                            TotalOrderPrice = dt.Rows[0]["TotalOrderPrice"] != DBNull.Value ? Convert.ToDecimal(dt.Rows[0]["TotalOrderPrice"]): 0

                    };
                    }

                    // Table 1: Product List
                    if (ds.Tables.Count > 1)
                    {
                        DataTable dtProducts = ds.Tables[1];
                        var productList = new List<PurchaseOrderProductModel>();

                        foreach (DataRow row in dtProducts.Rows)
                        {
                            productList.Add(new PurchaseOrderProductModel
                            {
                                ProductName = Convert.ToString(row["ProductName"]),
                                ModelNumber = Convert.ToString(row["ModelNumber"]),
                                Quantity = Convert.ToInt32(row["Quantity"]),
                                Price = Convert.ToInt32(row["Price"])
                            });
                        }

                        detailModel.Products = productList;
                    }

                    // Table 2: Document List
                    if (ds.Tables.Count > 2)
                    {
                        DataTable dtDocs = ds.Tables[2];
                        var documentList = new List<PurchaseOrderDocumentModel>();

                        foreach (DataRow row in dtDocs.Rows)
                        {
                            documentList.Add(new PurchaseOrderDocumentModel
                            {
                                DocumentName = Convert.ToString(row["DocumentName"]),
                                DocumentFileName = Convert.ToString(row["DocumentFileName"]),
                                Notes = Convert.ToString(row["Notes"])
                            });
                        }

                        detailModel.Documents = documentList;
                    }
                }
            }
            catch (Exception ex)
            {
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
                Console.WriteLine($"Error in PurchaseOrderGetDetails: {ex.Message}");
            }

            return detailModel;
        }

        public PurchaseOrderModel GetPurchaseOrderById(int id)
        {
            PurchaseOrderModel model = new PurchaseOrderModel();
            try
            {
                if (id != 0)
                {
                    DbCommand com = this.db.GetStoredProcCommand("PurchaseOrderGetEditData");
                    db.AddInParameter(com, "@PurchaseOrderId", DbType.Int32, id);

                    DataSet ds = this.db.ExecuteDataSet(com);

                    // PurchaseOrder Main Details
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = ds.Tables[0].Rows[0];

                        model.PurchaseOrderId = Convert.ToInt32(row["PurchaseOrderId"]);
                        model.PurchaseOrderSerialNumber = Convert.ToString(row["PurchaseOrderSerialNumber"]);
                        model.VendorId = Convert.ToInt32(row["VendorId"]);
                        model.VendorContactId = Convert.ToInt32(row["VendorContactId"]);
                        model.PurchaseOrderDate = Convert.ToDateTime(row["PurchaseOrderDate"]);
                        model.ExpectedDeliveryDate = Convert.ToDateTime(row["ExpectedDeliveryDate"]);
                        model.PurchaseOrderStatusId = Convert.ToInt32(row["LKPPurchaseOrderStatusId"]);
                        model.OrderNotes = Convert.ToString(row["OrderNotes"]);
                        model.PaymentTerms = Convert.ToString(row["PaymentTerms"]);
                        model.InvoiceReceived = Convert.ToInt32(row["InvoiceReceived"]);
                        model.CreatedBy = Convert.ToInt32(row["CreatedBy"]);
                        model.CreatedOn = Convert.ToDateTime(row["CreatedOn"]);
                        model.LastModifiedBy = Convert.ToInt32(row["LastModifiedBy"]);
                        model.LastModifiedOn = Convert.ToDateTime(row["LastModifiedOn"]);
                    }

                    // Products List
                    if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                    {
                        model.ProductsList = new List<PurchaseOrderProductModel>();
                        foreach (DataRow row in ds.Tables[1].Rows)
                        {
                            model.ProductsList.Add(new PurchaseOrderProductModel
                            {
                                ProductId = Convert.ToInt32(row["ProductId"]),
                                ProductName = Convert.ToString(row["ProductName"]),
                                Quantity = Convert.ToInt32(row["Quantity"]),
                                Price = Convert.ToInt32(row["Price"])
                            });
                        }
                    }

                    // Documents List
                    if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                    {
                        model.DocumentsList = new List<PurchaseOrderDocumentModel>();
                        foreach (DataRow row in ds.Tables[2].Rows)
                        {
                            model.DocumentsList.Add(new PurchaseOrderDocumentModel
                            {
                                DocumentName = Convert.ToString(row["DocumentName"]),
                                DocumentFileName = Convert.ToString(row["DocumentFileName"]),
                                Notes = Convert.ToString(row["Notes"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
                Console.WriteLine($"Error in GetPurchaseOrderById: {ex.Message}");
            }

            return model;
        }

        public int SavePurchaseOrder(PurchaseOrderModel detail)
        {
            int resultCode = -1;

            try
            {
                DbCommand cmd = db.GetStoredProcCommand("InsertOrUpdatePurchaseOrder");

                if (detail.PurchaseOrderId > 0)
                {
                    db.AddInParameter(cmd, "@PurchaseOrderId", DbType.Int32, detail.PurchaseOrderId);
                }
                else
                {
                    db.AddInParameter(cmd, "@PurchaseOrderId", DbType.Int32, 0);
                }

                if (detail.VendorId > 0)
                {
                    db.AddInParameter(cmd, "@VendorId", DbType.Int32, detail.VendorId);
                }
                else
                {
                    db.AddInParameter(cmd, "@VendorId", DbType.Int32, DBNull.Value);
                }

                if (detail.VendorContactId > 0)
                {
                    db.AddInParameter(cmd, "@VendorContactId", DbType.Int32, detail.VendorContactId);
                }
                else
                {
                    db.AddInParameter(cmd, "@VendorContactId", DbType.Int32, DBNull.Value);
                }

                if (!string.IsNullOrEmpty(detail.OrderNotes))
                {
                    db.AddInParameter(cmd, "@OrderNotes", DbType.String, detail.OrderNotes);
                }
                else
                {
                    db.AddInParameter(cmd, "@OrderNotes", DbType.String, DBNull.Value);
                }

                if (detail.PurchaseOrderDate != DateTime.MinValue)
                {
                    db.AddInParameter(cmd, "@PurchaseOrderDate", DbType.DateTime, detail.PurchaseOrderDate);
                }
                else
                {
                    db.AddInParameter(cmd, "@PurchaseOrderDate", DbType.DateTime, DBNull.Value);
                }

                if (detail.ExpectedDeliveryDate != DateTime.MinValue)
                {
                    db.AddInParameter(cmd, "@ExpectedDeliveryDate", DbType.DateTime, detail.ExpectedDeliveryDate);
                }
                else
                {
                    db.AddInParameter(cmd, "@ExpectedDeliveryDate", DbType.DateTime, DBNull.Value);
                }

                if (detail.PurchaseOrderStatusId > 0)
                {
                    db.AddInParameter(cmd, "@LKPPurchaseOrderStatusId", DbType.Int32, detail.PurchaseOrderStatusId);
                }
                else
                {
                    db.AddInParameter(cmd, "@LKPPurchaseOrderStatusId", DbType.Int32, DBNull.Value);
                }

                if (!string.IsNullOrEmpty(detail.PaymentTerms))
                {
                    db.AddInParameter(cmd, "@PaymentTerms", DbType.String, detail.PaymentTerms);
                }
                else
                {
                    db.AddInParameter(cmd, "@PaymentTerms", DbType.String, DBNull.Value);
                }

                db.AddInParameter(cmd, "@InvoiceReceived", DbType.Boolean, detail.InvoiceReceived);

                db.AddInParameter(cmd, "@CreatedBy", DbType.Int32, CurrentUserId);


                string productsXml = ConvertProductsToXml(detail.ProductsList);
                db.AddInParameter(cmd, "@ProductsXml", DbType.Xml, productsXml);

                string filesXml = ConvertFilesToXml(detail.DocumentsList);
                db.AddInParameter(cmd, "@DocumentsXml", DbType.Xml, filesXml);

                db.AddOutParameter(cmd, "@CurrentPurchaseOrderId", DbType.Int32, 4);

                db.ExecuteNonQuery(cmd);

                resultCode = Convert.ToInt32(db.GetParameterValue(cmd, "@CurrentPurchaseOrderId"));
                return resultCode;
            }
            catch (Exception ex)
            {
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
                return -1;
            }
        }




        private string ConvertProductsToXml(List<PurchaseOrderProductModel> products)
        {
            if (products == null || products.Count == 0)
                return "<Products></Products>";

            var sb = new StringBuilder();
            sb.Append("<Products>");
            foreach (var product in products)
            {
                sb.Append("<Product>");
                sb.AppendFormat("<ProductId>{0}</ProductId>", product.ProductId);
                sb.AppendFormat("<Quantity>{0}</Quantity>", product.Quantity);
                sb.AppendFormat("<Price>{0}</Price>", product.Price);
                sb.Append("</Product>");
            }
            sb.Append("</Products>");
            return sb.ToString();
        }

        private string ConvertFilesToXml(List<PurchaseOrderDocumentModel> documents)
        {
            if (documents == null || documents.Count == 0)
                return "<Documents></Documents>";

            var sb = new StringBuilder();
            sb.Append("<Documents>");
            foreach (var doc in documents)
            {
                sb.Append("<Document>");
                sb.AppendFormat("<DocumentName>{0}</DocumentName>", System.Security.SecurityElement.Escape(doc.DocumentName));
                sb.AppendFormat("<DocumentFileName>{0}</DocumentFileName>", System.Security.SecurityElement.Escape(doc.DocumentFileName));
                sb.AppendFormat("<Notes>{0}</Notes>", System.Security.SecurityElement.Escape(doc.Notes));
                sb.Append("</Document>");
            }
            sb.Append("</Documents>");
            return sb.ToString();
        }

    }
}
