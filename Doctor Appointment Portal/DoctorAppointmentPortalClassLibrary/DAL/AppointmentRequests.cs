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
                DbCommand com = db.GetStoredProcCommand("AppointmentRequestsGetList");
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
                if (model.StatusId > 0)
                {
                    db.AddInParameter(com, "@StatusId", DbType.Int32, model.StatusId);
                }
                else
                {
                    db.AddInParameter(com, "@StatusId", DbType.Int32, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(model.AppointmentType))
                {
                    db.AddInParameter(com, "@AppointmentType", DbType.String, model.AppointmentType);
                }
                else
                {
                    db.AddInParameter(com, "@AppointmentType", DbType.String, DBNull.Value);
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

        public List<AppointmentRequestsModel> GetDoctorAppointmentRequests(AppointmentRequestsModel model)
        {
            List<AppointmentRequestsModel> AppointmentRequestList = new List<AppointmentRequestsModel>();

            try
            {
                // Use the doctor-specific stored procedure
                DbCommand com = db.GetStoredProcCommand("DoctorAppointmentRequestsGetList");

                // DoctorId is required
                db.AddInParameter(com, "@DoctorId", DbType.Int32, model.DoctorId);

                // Patient Name (optional search)
                if (!string.IsNullOrEmpty(model.SearchedPatientName))
                {
                    db.AddInParameter(com, "@PatientName", DbType.String, model.SearchedPatientName);
                }
                else
                {
                    db.AddInParameter(com, "@PatientName", DbType.String, DBNull.Value);
                }

                // Specialization Id (optional)
                if (model.SpecializationId > 0)
                {
                    db.AddInParameter(com, "@SpecializationId", DbType.Int32, model.SpecializationId);
                }
                else
                {
                    db.AddInParameter(com, "@SpecializationId", DbType.Int32, DBNull.Value);
                }

                if (model.StatusId > 0)
                {
                    db.AddInParameter(com, "@StatusId", DbType.Int32, model.StatusId);
                }
                else
                {
                    db.AddInParameter(com, "@StatusId", DbType.Int32, DBNull.Value);
                }
                if (!string.IsNullOrEmpty(model.AppointmentType))
                {
                    db.AddInParameter(com, "@AppointmentType", DbType.String, model.AppointmentType);
                }
                else
                {
                    db.AddInParameter(com, "@AppointmentType", DbType.String, DBNull.Value);
                }

                // Start Date filter
                if (model.FromDate.HasValue)
                {
                    db.AddInParameter(com, "@FromDate", DbType.Date, model.FromDate);
                }
                else
                {
                    db.AddInParameter(com, "@FromDate", DbType.Date, DBNull.Value);
                }

                // End Date filter
                if (model.ToDate.HasValue)
                {
                    db.AddInParameter(com, "@ToDate", DbType.Date, model.ToDate.Value);
                }
                else
                {
                    db.AddInParameter(com, "@ToDate", DbType.Date, DBNull.Value);
                }

                // Page Number & Page Size
                db.AddInParameter(com, "@PageNumber", DbType.Int32, model.PageNumber);
                db.AddInParameter(com, "@PageSize", DbType.Int32, model.PageSize);

                // Output parameter for total count
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
                            DoctorEmail = Convert.ToString(row["DoctorEmail"]),
                            PatientEmail = Convert.ToString(row["PatientEmail"]),
                            MedicalConcern = Convert.ToString(row["MedicalConcern"]),
                            SpecializationId = row["SpecializationId"] != DBNull.Value ? Convert.ToInt32(row["SpecializationId"]) : 0,
                            SpecializationName = Convert.ToString(row["SpecializationName"]),
                            SlotId = row["SlotId"] != DBNull.Value ? Convert.ToInt32(row["SlotId"]) : 0,
                            FinalStartTime = row["FinalStartTime"] != DBNull.Value ? (TimeSpan)row["FinalStartTime"] : TimeSpan.Zero,
                            FinalEndTime = row["FinalEndTime"] != DBNull.Value ? (TimeSpan)row["FinalEndTime"] : TimeSpan.Zero,
                            FinalDate = row["FinalDate"] != DBNull.Value ? Convert.ToDateTime(row["FinalDate"]) : DateTime.MinValue,
                            StatusId = row["StatusId"] != DBNull.Value ? Convert.ToInt32(row["StatusId"]) : 0,
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
                Console.WriteLine($"Error in GetDoctorAppointmentRequests: {ex.Message}");
            }

            return AppointmentRequestList;
        }


        public bool UpdateAppointmentStatus(AppointmentRequestsModel model)
        {
            bool isUpdated = false;

            try
            {
                DbCommand com = db.GetStoredProcCommand("UpdateAppointmentStatus");

                db.AddInParameter(com, "@AppointmentRequestId", DbType.Int32, model.AppointmentRequestId);
                db.AddInParameter(com, "@StatusName", DbType.String, model.Action);
                db.AddInParameter(com, "@LastModifiedBy", DbType.Int32, model.LastModifiedBy);

                int rowsAffected = db.ExecuteNonQuery(com);

                isUpdated = rowsAffected > 0;
            }
            catch (Exception ex)
            {
                // Log error if needed
                Console.WriteLine($"Error in UpdateAppointmentStatus: {ex.Message}");
            }

            return isUpdated;
        }

        public bool RescheduleAppointment(AppointmentRequestsModel model)
        {
            bool isUpdated = false;

            try
            {
                // Create command for stored procedure
                DbCommand com = db.GetStoredProcCommand("RescheduleAppointment");

                // Add input parameters from model
                db.AddInParameter(com, "@AppointmentRequestId", DbType.Int32, model.AppointmentRequestId);
                db.AddInParameter(com, "@OldSlotId", DbType.Int32, model.OldSlotId);
                db.AddInParameter(com, "@NewSlotId", DbType.Int32, model.SlotId);
                db.AddInParameter(com, "@NewStartTime", DbType.String, model.StartTime.ToString(@"hh\:mm\:ss"));
                db.AddInParameter(com, "@NewEndTime", DbType.String, model.EndTime.ToString(@"hh\:mm\:ss"));
                db.AddInParameter(com, "@NewDate", DbType.Date, model.PreferredDate);
                db.AddInParameter(com, "@DoctorId", DbType.Int32, model.DoctorId);

                // Execute stored procedure
                int rowsAffected = db.ExecuteNonQuery(com);

                isUpdated = rowsAffected > 0;
            }
            catch (Exception ex)
            {
                // Log error if needed
                Console.WriteLine($"Error in RescheduleAppointment: {ex.Message}");
            }

            return isUpdated;
        }


        public AppointmentRequestsModel LoadPatientDetails(string contactNumber)
        {
            AppointmentRequestsModel model = new AppointmentRequestsModel();

            try
            {
                if (!string.IsNullOrEmpty(contactNumber))
                {
                    DbCommand com = this.db.GetStoredProcCommand("GetPatientPersonalInfoByPhone");
                    db.AddInParameter(com, "PhoneNumber", DbType.String, contactNumber);

                    DataSet ds = this.db.ExecuteDataSet(com);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];
                        model.PatientId = Convert.ToInt32(dt.Rows[0]["PatientId"]);
                        model.FirstName = Convert.ToString(dt.Rows[0]["FirstName"]);
                        model.LastName = Convert.ToString(dt.Rows[0]["LastName"]);
                        model.PatientEmail = Convert.ToString(dt.Rows[0]["Email"]);
                        model.ContactNumber = Convert.ToString(dt.Rows[0]["ContactNumber"]);
                        model.DateOfBirth = Convert.ToDateTime(dt.Rows[0]["DateOfBirth"]);
                        model.Gender = Convert.ToString(dt.Rows[0]["Gender"]);

                        // Address
                        model.AddressLine = Convert.ToString(dt.Rows[0]["AddressLine"]);
                        model.StateId = Convert.ToInt32(dt.Rows[0]["StateId"]);
                        model.DistrictId = Convert.ToInt32(dt.Rows[0]["DistrictId"]);
                        model.TalukaId = Convert.ToInt32(dt.Rows[0]["TalukaId"]);
                        model.CityId = Convert.ToInt32(dt.Rows[0]["CityId"]);
                        model.Pincode = Convert.ToString(dt.Rows[0]["Pincode"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoadPatientDetails: {ex.Message}");
                // Handle Exception
            }

            return model;
        }

        public int SavePatientAppointment(AppointmentRequestsModel appointment)
        {
            int resultCode = -1;

            try
            {
                DbCommand cmd = db.GetStoredProcCommand("SavePatientAppointment");

                // Patient Info
                if (!string.IsNullOrEmpty(appointment.FirstName))
                    db.AddInParameter(cmd, "@FirstName", DbType.String, appointment.FirstName);
                else
                    db.AddInParameter(cmd, "@FirstName", DbType.String, DBNull.Value);

                if (!string.IsNullOrEmpty(appointment.LastName))
                    db.AddInParameter(cmd, "@LastName", DbType.String, appointment.LastName);
                else
                    db.AddInParameter(cmd, "@LastName", DbType.String, DBNull.Value);

                if (!string.IsNullOrEmpty(appointment.PatientEmail))
                    db.AddInParameter(cmd, "@Email", DbType.String, appointment.PatientEmail);
                else
                    db.AddInParameter(cmd, "@Email", DbType.String, DBNull.Value);

                if (!string.IsNullOrEmpty(appointment.ContactNumber))
                    db.AddInParameter(cmd, "@PhoneNumber", DbType.String, appointment.ContactNumber);
                else
                    db.AddInParameter(cmd, "@PhoneNumber", DbType.String, DBNull.Value);

                if (appointment.DateOfBirth != DateTime.MinValue)
                    db.AddInParameter(cmd, "@DateOfBirth", DbType.Date, appointment.DateOfBirth);
                else
                    db.AddInParameter(cmd, "@DateOfBirth", DbType.Date, DBNull.Value);

                if (!string.IsNullOrEmpty(appointment.Gender))
                    db.AddInParameter(cmd, "@Gender", DbType.String, appointment.Gender);
                else
                    db.AddInParameter(cmd, "@Gender", DbType.String, DBNull.Value);

                if (!string.IsNullOrEmpty(appointment.InsuranceInfo))
                    db.AddInParameter(cmd, "@InsuranceInfo", DbType.String, appointment.InsuranceInfo);
                else
                    db.AddInParameter(cmd, "@InsuranceInfo", DbType.String, DBNull.Value);

                if (!string.IsNullOrEmpty(appointment.MedicalHistory))
                    db.AddInParameter(cmd, "@MedicalHistory", DbType.String, appointment.MedicalHistory);
                else
                    db.AddInParameter(cmd, "@MedicalHistory", DbType.String, DBNull.Value);

                if (!string.IsNullOrEmpty(appointment.AddressLine))
                    db.AddInParameter(cmd, "@AddressLine", DbType.String, appointment.AddressLine);
                else
                    db.AddInParameter(cmd, "@AddressLine", DbType.String, DBNull.Value);

                // Slot Info
                if (appointment.PreferredDate != DateTime.MinValue)
                    db.AddInParameter(cmd, "@SlotDate", DbType.Date, appointment.PreferredDate);
                else
                    db.AddInParameter(cmd, "@SlotDate", DbType.Date, DBNull.Value);

                if (appointment.StartTime != TimeSpan.Zero)
                    db.AddInParameter(cmd, "@StartTime", DbType.String, appointment.StartTime.ToString(@"hh\:mm\:ss"));
                else
                    db.AddInParameter(cmd, "@StartTime", DbType.String, DBNull.Value);

                if (appointment.EndTime != TimeSpan.Zero)
                    db.AddInParameter(cmd, "@EndTime", DbType.String, appointment.EndTime.ToString(@"hh\:mm\:ss"));
                else
                    db.AddInParameter(cmd, "@EndTime", DbType.String, DBNull.Value);

                if (appointment.SelectedSpecializationId > 0)
                    db.AddInParameter(cmd, "@SpecializationId", DbType.Int32, appointment.SelectedSpecializationId);
                else
                    db.AddInParameter(cmd, "@SpecializationId", DbType.Int32, DBNull.Value);

                // Address Info
                if (appointment.StateId > 0)
                    db.AddInParameter(cmd, "@StateId", DbType.Int32, appointment.StateId);
                else
                    db.AddInParameter(cmd, "@StateId", DbType.Int32, DBNull.Value);

                if (appointment.DistrictId > 0)
                    db.AddInParameter(cmd, "@DistrictId", DbType.Int32, appointment.DistrictId);
                else
                    db.AddInParameter(cmd, "@DistrictId", DbType.Int32, DBNull.Value);

                if (appointment.TalukaId > 0)
                    db.AddInParameter(cmd, "@TalukaId", DbType.Int32, appointment.TalukaId);
                else
                    db.AddInParameter(cmd, "@TalukaId", DbType.Int32, DBNull.Value);

                if (appointment.CityId > 0)
                    db.AddInParameter(cmd, "@CityId", DbType.Int32, appointment.CityId);
                else
                    db.AddInParameter(cmd, "@CityId", DbType.Int32, DBNull.Value);

                if (!string.IsNullOrEmpty(appointment.Pincode))
                    db.AddInParameter(cmd, "@Pincode", DbType.String, appointment.Pincode);
                else
                    db.AddInParameter(cmd, "@Pincode", DbType.String, DBNull.Value);

                // Doctor and Slot
                if (appointment.DoctorId > 0)
                    db.AddInParameter(cmd, "@DoctorId", DbType.Int32, appointment.DoctorId);
                else
                    db.AddInParameter(cmd, "@DoctorId", DbType.Int32, DBNull.Value);

                if (appointment.SlotId > 0)
                    db.AddInParameter(cmd, "@SlotId", DbType.Int32, appointment.SlotId);
                else
                    db.AddInParameter(cmd, "@SlotId", DbType.Int32, DBNull.Value);

                if (!string.IsNullOrEmpty(appointment.MedicalConcern))
                    db.AddInParameter(cmd, "@MedicalConcern", DbType.String, appointment.MedicalConcern);
                else
                    db.AddInParameter(cmd, "@MedicalConcern", DbType.String, DBNull.Value);
                if (appointment.UploadedFile != null && !string.IsNullOrEmpty(appointment.UploadedFile.FileName))
                {
                    db.AddInParameter(cmd, "@DocumentFileName", DbType.String, appointment.UploadedFile.FileName);
                }
                else
                {
                    db.AddInParameter(cmd, "@DocumentFileName", DbType.String, DBNull.Value);
                }


                // Saved file path (relative path on server)
                if (appointment.UploadedFile != null && !string.IsNullOrEmpty(appointment.UploadedFile.FilePath))
                {
                    db.AddInParameter(cmd, "@DocumentFilePath", DbType.String, appointment.UploadedFile.FilePath);
                }
                else
                {
                    db.AddInParameter(cmd, "@DocumentFilePath", DbType.String, DBNull.Value);
                }


                // Output parameter for inserted appointment ID
                db.AddOutParameter(cmd, "@AppointmentId", DbType.Int32, 4);

                db.ExecuteNonQuery(cmd);

                resultCode = Convert.ToInt32(db.GetParameterValue(cmd, "@AppointmentId"));
                return resultCode;
            }
            catch (Exception ex)
            {
                // errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace, appointment.CreatedBy);
                return -1;
            }
        }

    }


}
