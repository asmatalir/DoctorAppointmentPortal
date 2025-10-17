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
    public class Users
    {
        private Database db;
        public Users()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        ErrorLogs errorLogsDAL = new ErrorLogs();
        public int CurrentUserId { get; set; }

        public bool GetUserByUserName(UserInfoModel model)
        {
            try
            {
                DbCommand com = db.GetStoredProcCommand("GetUserByUserName");

                db.AddInParameter(com, "UserName", DbType.String, model.UserName);
                DataSet ds = db.ExecuteDataSet(com);
                
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];

                    model.PrimaryPassword = Convert.ToString(row["PrimaryPassword"]);
                    model.Email = Convert.ToString(row["Email"]);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
                return false;
            }
        }

        public bool UpdateSecondaryPassword(string userName, string secondaryPassword)
        {
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("UpdateSecondaryPassword");

                db.AddInParameter(cmd, "UserName", DbType.String, userName);
                db.AddInParameter(cmd, "SecondaryPassword", DbType.String, secondaryPassword);

                int rowsAffected = db.ExecuteNonQuery(cmd);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
                return false;
            }
        }

        public UserInfoModel GetSecondaryPasswordByUserName(string userName)
        {
            try
            {
                DbCommand com = db.GetStoredProcCommand("GetSecondaryPasswordByUserName");
                db.AddInParameter(com, "UserName", DbType.String, userName);

                UserInfoModel result = new UserInfoModel();
                DataSet ds = db.ExecuteDataSet(com);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    result.SecondaryPassword = Convert.ToString(ds.Tables[0].Rows[0]["SecondaryPassword"]);
                    result.UserId = Convert.ToInt32(ds.Tables[0].Rows[0]["UserId"]);
                }
                return result;
            }
            catch (Exception ex)
            {
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
            }

            return null;
        }


    }
}
