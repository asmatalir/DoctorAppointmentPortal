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
    public class Specializations
    {
        private Database db;

        public Specializations()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        //ErrorLogs errorLogsDAL = new ErrorLogs();

        public List<SpecializationsModel> GetList()
        {
            List<SpecializationsModel> specializationList = new List<SpecializationsModel>();

            try
            {
                DbCommand com = db.GetStoredProcCommand("DoctorSpecializationGetList");
                DataSet ds = db.ExecuteDataSet(com);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        SpecializationsModel specialization = new SpecializationsModel()
                        {
                            SpecializationId = Convert.ToInt32(row["SpecializationId"]),
                            SpecializationName = Convert.ToString(row["SpecializationName"]),
                            IsActive = Convert.ToBoolean(row["IsActive"])
                        };

                        specializationList.Add(specialization);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return specializationList;
        }
    }
}
