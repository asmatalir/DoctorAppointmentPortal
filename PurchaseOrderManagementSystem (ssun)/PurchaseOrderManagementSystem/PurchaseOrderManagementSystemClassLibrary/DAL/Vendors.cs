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
    public class Vendors
    {
        private Database db;

        public Vendors()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        ErrorLogs errorLogsDAL = new ErrorLogs();
        public List<VendorModel> GetList()
        {

            List<VendorModel> VendorsList = new List<VendorModel>();
            try
            {
                DbCommand com = db.GetStoredProcCommand("VendorsGetList");
                DataSet ds = db.ExecuteDataSet(com);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        VendorModel vendors = new VendorModel()
                        {
                            VendorId = Convert.ToInt32(row["VendorId"]),
                            VendorName = Convert.ToString(row["VendorName"]),
                            Address = Convert.ToString(row["Address"]),
                            City = Convert.ToString(row["City"]),
                            State = Convert.ToString(row["State"]),
                            PostCode = Convert.ToString(row["PostCode"]),
                            Email = Convert.ToString(row["Email"]),
                            Mobile = Convert.ToString(row["mobile"]),
                            Website = Convert.ToString(row["Website"]),
                        };
                        VendorsList.Add(vendors);
                    }
                }
            }
            catch (Exception ex)
            {
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
                Console.WriteLine($"Error in GetList: {ex.Message}");

            }

            return VendorsList;
        }
    }
}
