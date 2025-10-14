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
    public class DoctorAvailabilityExceptions
    {
        private Database db;

        public DoctorAvailabilityExceptions()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        public List<DoctorAvailabilityExceptionsModel> GetDetails(int DoctorId)
        {
            List<DoctorAvailabilityExceptionsModel> ExceptionList = new List<DoctorAvailabilityExceptionsModel>();

            try
            {
                DbCommand com = db.GetStoredProcCommand("DoctorAvailabilityExceptionsGetList");
                db.AddInParameter(com, "@DoctorId", DbType.String, DoctorId);
                DataSet ds = db.ExecuteDataSet(com);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        DoctorAvailabilityExceptionsModel exception = new DoctorAvailabilityExceptionsModel()
                        {
                            ExceptionId = Convert.ToInt32(row["ExceptionId"]),
                            ExceptionDate = Convert.ToDateTime(row["ExceptionDate"]),
                            StartTime = (TimeSpan)row["StartTime"],
                            EndTime = (TimeSpan)row["EndTime"],
                            IsAvailable = Convert.ToInt32(row["IsAvailable"]),
                            Reason = Convert.ToString(row["Reason"]),
                            IsActive = Convert.ToBoolean(row["IsActive"]),
                        };

                        ExceptionList.Add(exception);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetList: {ex.Message}");
            }

            return ExceptionList;
        }
    }
}
