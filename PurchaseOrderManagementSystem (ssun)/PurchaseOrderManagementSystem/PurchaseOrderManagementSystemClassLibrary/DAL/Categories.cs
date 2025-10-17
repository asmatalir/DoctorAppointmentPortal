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
    public class Categories
    {
        private Database db;

        public Categories()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        ErrorLogs errorLogsDAL = new ErrorLogs();
        public List<ProductCategoryModel> GetList()
        {

            List<ProductCategoryModel> ProductCategoryList = new List<ProductCategoryModel>();
            try
            {
                DbCommand com = db.GetStoredProcCommand("ProductCategoryGetList");
                DataSet ds = db.ExecuteDataSet(com);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        ProductCategoryModel categories = new ProductCategoryModel()
                        {
                            ProductCategoryId = Convert.ToInt32(row["LKPProductCategoryId"]),
                            ProductCategory = Convert.ToString(row["ProductCategory"]),
                            Description = Convert.ToString(row["Description"]),
                        };
                        ProductCategoryList.Add(categories);
                    }
                }
            }
            catch (Exception ex)
            {
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
                Console.WriteLine($"Error in GetList: {ex.Message}");

            }

            return ProductCategoryList;
        }
    }
}
