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
    public class DoctorAvailableSlots
    {
        private Database db;

        public DoctorAvailableSlots()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        public List<DoctorAvailableSlotsModel> GetSlots(int doctorId)
        {
            List<DoctorAvailableSlotsModel> slotsList = new List<DoctorAvailableSlotsModel>();

            try
            {
                DbCommand cmd = db.GetStoredProcCommand("GetDoctorAvailableSlots");
                if (doctorId > 0)
                    db.AddInParameter(cmd, "@DoctorId", DbType.Int32, doctorId);
                else
                    db.AddInParameter(cmd, "@DoctorId", DbType.Int32, DBNull.Value);

                DataSet ds = db.ExecuteDataSet(cmd);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    foreach (DataRow row in dt.Rows)
                    {
                        DoctorAvailableSlotsModel slot = new DoctorAvailableSlotsModel()
                        {
                            SlotId = Convert.ToInt32(row["SlotId"]),
                            DoctorId = Convert.ToInt32(row["DoctorId"]),
                            SlotDate = Convert.ToDateTime(row["SlotDate"]),
                            StartTime = (TimeSpan)row["StartTime"],
                            EndTime = (TimeSpan)row["EndTime"],
                            StatusId = Convert.ToInt32(row["StatusId"]),
                            StatusName = row["StatusName"].ToString()
                        };

                        slotsList.Add(slot);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return slotsList;
        }
    }
}
