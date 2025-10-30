using DoctorAppointmentPortalClassLibrary.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace DoctorAppointmentPortalClassLibrary.DAL
{
    public class Qualifications
    {
        private Database db;

        public Qualifications()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        // ErrorLogs errorLogsDAL = new ErrorLogs();

        public List<QualificationsModel> GetList()
        {
            List<QualificationsModel> qualificationList = new List<QualificationsModel>();

            try
            {
                DbCommand com = db.GetStoredProcCommand("DoctorQualificationsGetList");
                DataSet ds = db.ExecuteDataSet(com);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        QualificationsModel qualification = new QualificationsModel()
                        {
                            QualificationId = Convert.ToInt32(row["QualificationId"]),
                            QualificationName = Convert.ToString(row["QualificationName"]),
                            IsActive = Convert.ToBoolean(row["IsActive"])
                        };

                        qualificationList.Add(qualification);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return qualificationList;
        }
    }
}
