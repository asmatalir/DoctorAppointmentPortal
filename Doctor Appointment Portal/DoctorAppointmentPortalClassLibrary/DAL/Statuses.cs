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
    public class Statuses
    {

        private Database db;

        public Statuses()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        public List<StatusesModel> GetList()
        {
            List<StatusesModel> statusesList = new List<StatusesModel>();

            try
            {
                DbCommand com = db.GetStoredProcCommand("StatusesGetList");
                DataSet ds = db.ExecuteDataSet(com);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        StatusesModel statuses = new StatusesModel()
                        {
                            StatusId = Convert.ToInt32(row["StatusId"]),
                            StatusName = Convert.ToString(row["StatusName"]),
                            IsActive = Convert.ToBoolean(row["IsActive"])
                        };

                        statusesList.Add(statuses);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetList: {ex.Message}");
            }

            return statusesList;
        }
    }
}
