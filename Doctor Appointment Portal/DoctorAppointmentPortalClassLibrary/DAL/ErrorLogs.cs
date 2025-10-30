using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentPortalClassLibrary.DAL
{
    public class ErrorLogs
    {
        private Database db;

        public ErrorLogs()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        public int CurrentUserId { get; set; }
        public void InsertErrorLogs(string ErrorMessage, string StackTrace)
        {
            try
            {
                DbCommand com = db.GetStoredProcCommand("DoctorAppointmentErrorLogsInsert");
                db.AddInParameter(com, "@ErrorMessage", DbType.String, ErrorMessage);
                db.AddInParameter(com, "@StackTrace", DbType.String, StackTrace);
                db.AddInParameter(com, "@CreatedBy", DbType.Int32, CurrentUserId);
                db.ExecuteNonQuery(com);
            }
            catch (Exception ex)
            {
                //throw; 
            }
        }
    }
}
