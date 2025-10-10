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
    public class Doctors
    {
        private Database db;

        public Doctors()
        {
            db = DatabaseFactory.CreateDatabase();
        }
        //ErrorLogs errorLogsDAL = new ErrorLogs();
        public List<DoctorsModel> GetList()
        {

            List<DoctorsModel> DoctorsList = new List<DoctorsModel>();
            try
            {
                DbCommand com = db.GetStoredProcCommand("DoctorsGetList");
                DataSet ds = db.ExecuteDataSet(com);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        DoctorsModel doctors = new DoctorsModel()
                        {
                            DoctorId = Convert.ToInt32(row["DoctorId"]),
                            FirstName = Convert.ToString(row["FirstName"]),
                            LastName = Convert.ToString(row["LastName"]),
                            Gender = Convert.ToString(row["Gender"]),
                            ExperienceYears = Convert.ToInt32(row["ExperienceYears"]),
                            ConsultationFees = Convert.ToDecimal(row["ConsultationFees"]),
                            Description = Convert.ToString(row["Description"]),
                            HospitalName = Convert.ToString(row["HospitalName"]),
                            Address = Convert.ToString(row["Address"]),
                            Rating = Convert.ToDecimal(row["Rating"]),
                            IsActive = Convert.ToBoolean(row["IsActive"])
                        };
                        DoctorsList.Add(doctors);
                    }
                }
            }
            catch (Exception ex)
            {
                //errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace, 1);
                Console.WriteLine($"Error in GetList: {ex.Message}");
            }

            return DoctorsList;
        }

    }
}
