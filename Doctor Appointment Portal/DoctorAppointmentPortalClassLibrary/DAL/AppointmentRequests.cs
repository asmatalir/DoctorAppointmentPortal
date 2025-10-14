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
    public class AppointmentRequests
    {
        private Database db;
        public int TotalRecords { get; set; }
        public AppointmentRequests()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        public List<AppointmentRequestsModel> GetList(AppointmentRequestsModel model)
        {
            List<AppointmentRequestsModel> AppointmentRequestList = new List<AppointmentRequestsModel>();

            try
            {
                DbCommand com = db.GetStoredProcCommand("AppointmentRequestsGetListFiltered");
                // Patient Name
                if (!string.IsNullOrEmpty(model.SearchedPatientName))
                {
                    db.AddInParameter(com, "@PatientName", DbType.String, model.SearchedPatientName);
                }
                else
                {
                    db.AddInParameter(com, "@PatientName", DbType.String, DBNull.Value);
                }

                // Doctor Name
                if (!string.IsNullOrEmpty(model.SearchedDoctorName))
                {
                    db.AddInParameter(com, "@DoctorName", DbType.String, model.SearchedDoctorName);
                }
                else
                {
                    db.AddInParameter(com, "@DoctorName", DbType.String, DBNull.Value);
                }

                // Specialization Id
                if (model.SpecializationId > 0)
                {
                    db.AddInParameter(com, "@SpecializationId", DbType.Int32, model.SpecializationId);
                }
                else
                {
                    db.AddInParameter(com, "@SpecializationId", DbType.Int32, DBNull.Value);
                }

                // Start Date
                if (model.FromDate.HasValue)
                {
                    db.AddInParameter(com, "@FromDate", DbType.Date, model.FromDate);
                }
                else
                {
                    db.AddInParameter(com, "@FromDate", DbType.Date, DBNull.Value);
                }

                // End Date
                if (model.ToDate.HasValue)
                {
                    db.AddInParameter(com, "@ToDate", DbType.Date, model.ToDate.Value);
                }
                else
                {
                    db.AddInParameter(com, "@ToDate", DbType.Date, DBNull.Value);
                }

                // Page Number
                db.AddInParameter(com, "@PageNumber", DbType.Int32, model.PageNumber);

                // Page Size
                db.AddInParameter(com, "@PageSize", DbType.Int32, model.PageSize);


                // Add output parameter for total count
                db.AddOutParameter(com, "@TotalCount", DbType.Int32, sizeof(int));

                DataSet ds = db.ExecuteDataSet(com);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    foreach (DataRow row in dt.Rows)
                    {
                        AppointmentRequestsModel appointment = new AppointmentRequestsModel()
                        {
                            AppointmentRequestId = Convert.ToInt32(row["AppointmentRequestId"]),
                            PatientId = Convert.ToInt32(row["PatientId"]),
                            PatientName = Convert.ToString(row["PatientName"]),
                            DoctorId = Convert.ToInt32(row["DoctorId"]),
                            DoctorName = Convert.ToString(row["DoctorName"]),
                            MedicalConcern = Convert.ToString(row["MedicalConcern"]),
                            PreferredDate = Convert.ToDateTime(row["PreferredDate"]),
                            SpecializationId = Convert.ToInt32(row["SpecializationId"]),
                            SpecializationName = Convert.ToString(row["SpecializationName"]),
                            StartTime = row["StartTime"] != DBNull.Value ? (TimeSpan)row["StartTime"] : TimeSpan.Zero,
                            EndTime = row["EndTime"] != DBNull.Value ? (TimeSpan)row["EndTime"] : TimeSpan.Zero,
                            FinalStartTime = row["FinalStartTime"] != DBNull.Value ? (TimeSpan)row["FinalStartTime"] : TimeSpan.Zero,
                            FinalEndTime = row["FinalEndTime"] != DBNull.Value ? (TimeSpan)row["FinalEndTime"] : TimeSpan.Zero,
                            FinalDate = row["FinalDate"] != DBNull.Value ? Convert.ToDateTime(row["FinalDate"]) : DateTime.MinValue,
                            StatusId = Convert.ToInt32(row["StatusId"]),
                            StatusName = Convert.ToString(row["StatusName"]),
                        };

                        AppointmentRequestList.Add(appointment);
                    }
                }

                // Get total count from output parameter
                this.TotalRecords = Convert.ToInt32(db.GetParameterValue(com, "@TotalCount"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetList: {ex.Message}");
            }

            return AppointmentRequestList;
        }
    }
}
