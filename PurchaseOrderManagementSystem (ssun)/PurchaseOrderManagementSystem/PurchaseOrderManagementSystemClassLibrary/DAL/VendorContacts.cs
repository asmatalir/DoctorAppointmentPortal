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
    public class VendorContacts
    {
        private Database db;

        public VendorContacts()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        ErrorLogs errorLogsDAL = new ErrorLogs();
        public List<VendorContactModel> GetList()
        {

            List<VendorContactModel> VendorContactList = new List<VendorContactModel>();
            try
            {
                DbCommand com = db.GetStoredProcCommand("VendorContactsGetList");
                DataSet ds = db.ExecuteDataSet(com);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        VendorContactModel vendorContacts = new VendorContactModel()
                        {
                            VendorContactId = Convert.ToInt32(row["VendorContactId"]),
                            Name = Convert.ToString(row["Name"]),
                            Designation = Convert.ToString(row["Designation"]),
                            Email = Convert.ToString(row["Email"]),
                            Mobile = Convert.ToString(row["mobile"]),
                        };
                        VendorContactList.Add(vendorContacts);
                    }
                }
            }
            catch (Exception ex)
            {
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
            }

            return VendorContactList;
        }

        public List<VendorContactModel> GetVendorContactsByVendorId(int vendorId)
        {
            List<VendorContactModel> contactList = new List<VendorContactModel>();

            try
            {
                using (DbCommand cmd = db.GetStoredProcCommand("GetVendorContactsByVendorId"))
                {
                    db.AddInParameter(cmd, "@VendorId", DbType.Int32, vendorId);

                    using (IDataReader reader = db.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            contactList.Add(new VendorContactModel
                            {
                                VendorContactId = reader["VendorContactId"] != DBNull.Value ? Convert.ToInt32(reader["VendorContactId"]) : 0,
                                Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : string.Empty
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
                throw new Exception("Error fetching vendor contacts by VendorId", ex);
            }

            return contactList;
        }
        }
}
