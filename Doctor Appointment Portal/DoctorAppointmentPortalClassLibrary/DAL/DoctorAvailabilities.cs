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
    public class DoctorAvailabilities
    {
        private Database db;

        public DoctorAvailabilities()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        public List<DoctorAvailabilitiesModel> GetDetails(int DoctorId)
        {
            List<DoctorAvailabilitiesModel> AvailabilityList = new List<DoctorAvailabilitiesModel>();

            try
            {
                DbCommand com = db.GetStoredProcCommand("DoctorAvailabilityGetList");
                db.AddInParameter(com, "@DoctorId", DbType.String, DoctorId);
                DataSet ds = db.ExecuteDataSet(com);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        DoctorAvailabilitiesModel availability = new DoctorAvailabilitiesModel()
                        {
                            DoctorAvailabilityId = Convert.ToInt32(row["DoctorAvailabilityId"]),
                            DayOfWeek = Convert.ToInt32(row["DayOfWeek"]),
                            StartTime = (TimeSpan)row["StartTime"],
                            EndTime = (TimeSpan)row["EndTime"],
                            Duration = Convert.ToInt32(row["SlotDuration"]),
                            IsActive = Convert.ToBoolean(row["IsActive"])
                           
                        };

                        AvailabilityList.Add(availability);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetList: {ex.Message}");
            }

            return AvailabilityList;
        }
    }
}
