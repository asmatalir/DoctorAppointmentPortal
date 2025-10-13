using Microsoft.Practices.EnterpriseLibrary.Data;
using PurchaseOrderManagementSystemClassLibrary.Models;
using System;
using System.Data;
using System.Data.Common;

namespace PurchaseOrderManagementSystemClassLibrary.DAL
{
    public class UserProfiles
    {
        private Database db;
        // Uncomment if you want error logging
        // ErrorLogs errorLogsDAL = new ErrorLogs();

        public UserProfiles()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        public int CurrentUserId { get; set; }

        public bool ValidateUserProfile(UserProfileModel model)
        {
            try
            {
                DbCommand com = db.GetStoredProcCommand("GetUserPasswordByUsername");

                db.AddInParameter(com, "UserName", DbType.String, model.UserName);

                DataSet ds = db.ExecuteDataSet(com);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];

                    // Just fetch the values, no comparison
                    model.HashedPassword = Convert.ToString(row["HashedPassword"]);
                    model.Email = Convert.ToString(row["Email"]);
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Uncomment to log errors
                // errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace, CurrentUserId);
                return false;
            }

            return false;
        }
    }
}
