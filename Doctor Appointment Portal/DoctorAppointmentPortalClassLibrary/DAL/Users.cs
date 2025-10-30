using DoctorAppointmentPortalClassLibrary.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentPortalClassLibrary.DAL
{
    public class Users
    {
        private Database db;

        public Users()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        //ErrorLogs errorLogsDAL = new ErrorLogs();
        public bool ValidateUser(UserProfilesModel model)
        {
            try
            {
                DbCommand com = db.GetStoredProcCommand("GetUserPasswordByUsername");
                if (!string.IsNullOrEmpty(model.UserName))
                    db.AddInParameter(com, "UserName", DbType.String, model.UserName);
                else
                    db.AddInParameter(com, "UserName", DbType.String, DBNull.Value);

                DataSet ds = db.ExecuteDataSet(com);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];

                    model.UserId = Convert.ToInt32(row["UserId"]);
                    model.FetchedPassword = row["HashedPassword"].ToString();
                    model.UserRoleId = Convert.ToInt32(row["UserRoleId"]);
                    model.RoleName = row["RoleName"].ToString();
                    model.Email = row["Email"].ToString();
                    model.UserName = row["UserName"].ToString();
                    model.DoctorId = !Convert.IsDBNull(row["DoctorId"]) ? Convert.ToInt32(row["DoctorId"]) : 0;
                    return true; 
                }
            }
            catch (Exception ex)
            {
                //errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
                return false;
            }

            return false;
        }
    }
}
