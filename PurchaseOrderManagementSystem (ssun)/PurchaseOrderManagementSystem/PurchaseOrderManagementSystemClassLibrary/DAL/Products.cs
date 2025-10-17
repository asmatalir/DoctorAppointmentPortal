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
    public class Products
    {
        private Database db;

        public Products()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        ErrorLogs errorLogsDAL = new ErrorLogs();
        public List<ProductModel> GetList()
        {

            List<ProductModel> ProductList = new List<ProductModel>();
            try
            {
                DbCommand com = db.GetStoredProcCommand("ProductGetList");
                DataSet ds = db.ExecuteDataSet(com);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        ProductModel products = new ProductModel()
                        {
                            ProductId = Convert.ToInt32(row["ProductId"]),
                            ProductName = Convert.ToString(row["ProductName"]),
                            ProductDescription = Convert.ToString(row["ProductDescription"]),
                            ModelNumber = Convert.ToString(row["ModelNumber"]),
                        };
                        ProductList.Add(products);
                    }
                }
            }
            catch (Exception ex)
            {
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
                Console.WriteLine($"Error in GetList: {ex.Message}");
            }

            return ProductList;
        }
    }
}
