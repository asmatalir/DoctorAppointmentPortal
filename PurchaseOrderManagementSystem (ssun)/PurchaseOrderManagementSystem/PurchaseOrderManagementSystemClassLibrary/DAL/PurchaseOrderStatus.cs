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
    public class PurchaseOrderStatus
    {
        private Database db;

        public PurchaseOrderStatus()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        ErrorLogs errorLogsDAL = new ErrorLogs();
        public List<PurchaseOrderStatusModel> GetList()
        {

            List<PurchaseOrderStatusModel> PurchaseOrderStatusList = new List<PurchaseOrderStatusModel>();
            try
            {
                DbCommand com = db.GetStoredProcCommand("PurchaseOrderStatusGetList");
                DataSet ds = db.ExecuteDataSet(com);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        PurchaseOrderStatusModel status = new PurchaseOrderStatusModel()
                        {
                            PurchaseOrderStatusId = Convert.ToInt32(row["LKPPurchaseOrderStatusId"]),
                            PurchaseOrderStatus = Convert.ToString(row["PurchaseOrderStatus"]),
                        };
                        PurchaseOrderStatusList.Add(status);
                    }
                }
            }
            catch (Exception ex)
            {
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
                Console.WriteLine($"Error in GetList: {ex.Message}");
            }

            return PurchaseOrderStatusList;
        }
    }
}
