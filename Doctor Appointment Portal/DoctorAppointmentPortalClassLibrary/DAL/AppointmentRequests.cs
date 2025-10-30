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
        ErrorLogs errorLogsDAL = new ErrorLogs();
        public List<AppointmentRequestsModel> GetList(AppointmentRequestsModel model)
        {
            List<AppointmentRequestsModel> AppointmentRequestList = new List<AppointmentRequestsModel>();

            try
            {
                DbCommand com = db.GetStoredProcCommand("AppointmentRequestsGetList");
                
                if (!string.IsNullOrEmpty(model.SearchedPatientName))
                {
                    db.AddInParameter(com, "@PatientName", DbType.String, model.SearchedPatientName);
                }
                else
                {
                    db.AddInParameter(com, "@PatientName", DbType.String, DBNull.Value);
                }

                
                if (!string.IsNullOrEmpty(model.SearchedDoctorName))
                {
                    db.AddInParameter(com, "@DoctorName", DbType.String, model.SearchedDoctorName);
                }
                else
                {
                    db.AddInParameter(com, "@DoctorName", DbType.String, DBNull.Value);
                }

                
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

                
                if (model.FromDate.HasValue)
                {
                    db.AddInParameter(com, "@FromDate", DbType.Date, model.FromDate);
                }
                else
                {
                    db.AddInParameter(com, "@FromDate", DbType.Date, DBNull.Value);
                }

                
                if (model.ToDate.HasValue)
                {
                    db.AddInParameter(com, "@ToDate", DbType.Date, model.ToDate.Value);
                }
                else
                {
                    db.AddInParameter(com, "@ToDate", DbType.Date, DBNull.Value);
                }

                
                db.AddInParameter(com, "@PageNumber", DbType.Int32, model.PageNumber);

                
                db.AddInParameter(com, "@PageSize", DbType.Int32, model.PageSize);


                
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

                
                this.TotalRecords = Convert.ToInt32(db.GetParameterValue(com, "@TotalCount"));
            }
            catch (Exception ex)
            {
                throw; 
            }

            return AppointmentRequestList;
        }

        public List<AppointmentRequestsModel> GetDoctorAppointmentRequests(AppointmentRequestsModel model)
        {
            List<AppointmentRequestsModel> AppointmentRequestList = new List<AppointmentRequestsModel>();

            try
            {
                
                DbCommand com = db.GetStoredProcCommand("DoctorAppointmentRequestsGetList");

                
                db.AddInParameter(com, "@DoctorId", DbType.Int32, model.DoctorId);

                
                if (!string.IsNullOrEmpty(model.SearchedPatientName))
                {
                    db.AddInParameter(com, "@PatientName", DbType.String, model.SearchedPatientName);
                }
                else
                {
                    db.AddInParameter(com, "@PatientName", DbType.String, DBNull.Value);
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

                
                if (model.FromDate.HasValue)
                {
                    db.AddInParameter(com, "@FromDate", DbType.Date, model.FromDate);
                }
                else
                {
                    db.AddInParameter(com, "@FromDate", DbType.Date, DBNull.Value);
                }

                
                if (model.ToDate.HasValue)
                {
                    db.AddInParameter(com, "@ToDate", DbType.Date, model.ToDate.Value);
                }
                else
                {
                    db.AddInParameter(com, "@ToDate", DbType.Date, DBNull.Value);
                }

                
                db.AddInParameter(com, "@PageNumber", DbType.Int32, model.PageNumber);
                db.AddInParameter(com, "@PageSize", DbType.Int32, model.PageSize);

                
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

                this.TotalRecords = Convert.ToInt32(db.GetParameterValue(com, "@TotalCount"));
            }
            catch (Exception ex)
            {
                throw; 
            }

            return AppointmentRequestList;
        }


        public bool UpdateAppointmentStatus(AppointmentRequestsModel model)
        {
            bool isUpdated = false;

            try
            {
                DbCommand com = db.GetStoredProcCommand("UpdateAppointmentStatus");

                if (model.AppointmentRequestId > 0)
                    db.AddInParameter(com, "@AppointmentRequestId", DbType.Int32, model.AppointmentRequestId);
                else
                    db.AddInParameter(com, "@AppointmentRequestId", DbType.Int32, DBNull.Value);

                if (!string.IsNullOrEmpty(model.Action))
                    db.AddInParameter(com, "@StatusName", DbType.String, model.Action);
                else
                    db.AddInParameter(com, "@StatusName", DbType.String, DBNull.Value);

                if (model.LastModifiedBy > 0)
                    db.AddInParameter(com, "@LastModifiedBy", DbType.Int32, model.LastModifiedBy);
                else
                    db.AddInParameter(com, "@LastModifiedBy", DbType.Int32, DBNull.Value);

                int rowsAffected = db.ExecuteNonQuery(com);

                isUpdated = rowsAffected > 0;
            }
            catch (Exception ex)
            {
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
            }

            return isUpdated;
        }

        public bool RescheduleAppointment(AppointmentRequestsModel model)
        {
            bool isUpdated = false;

            try
            {
                
                DbCommand com = db.GetStoredProcCommand("RescheduleAppointment");

                if (model.AppointmentRequestId > 0)
                    db.AddInParameter(com, "@AppointmentRequestId", DbType.Int32, model.AppointmentRequestId);
                else
                    db.AddInParameter(com, "@AppointmentRequestId", DbType.Int32, DBNull.Value);

                if (model.OldSlotId > 0)
                    db.AddInParameter(com, "@OldSlotId", DbType.Int32, model.OldSlotId);
                else
                    db.AddInParameter(com, "@OldSlotId", DbType.Int32, DBNull.Value);

                if (model.SlotId > 0)
                    db.AddInParameter(com, "@NewSlotId", DbType.Int32, model.SlotId);
                else
                    db.AddInParameter(com, "@NewSlotId", DbType.Int32, DBNull.Value);

                if (model.StartTime != TimeSpan.Zero)
                    db.AddInParameter(com, "@NewStartTime", DbType.String, model.StartTime.ToString(@"hh\:mm\:ss"));
                else
                    db.AddInParameter(com, "@NewStartTime", DbType.String, DBNull.Value);

                if (model.EndTime != TimeSpan.Zero)
                    db.AddInParameter(com, "@NewEndTime", DbType.String, model.EndTime.ToString(@"hh\:mm\:ss"));
                else
                    db.AddInParameter(com, "@NewEndTime", DbType.String, DBNull.Value);


                if (model.PreferredDate != DateTime.MinValue)
                    db.AddInParameter(com, "@NewDate", DbType.Date, model.PreferredDate);
                else
                    db.AddInParameter(com, "@NewDate", DbType.Date, DBNull.Value);


                if (model.DoctorId > 0)
                    db.AddInParameter(com, "@DoctorId", DbType.Int32, model.DoctorId);
                else
                    db.AddInParameter(com, "@DoctorId", DbType.Int32, DBNull.Value);


                int rowsAffected = db.ExecuteNonQuery(com);

                isUpdated = rowsAffected > 0;
            }
            catch (Exception ex)
            {
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
            }

            return isUpdated;
        }


        public AppointmentRequestsModel LoadPatientDetails(string aadhaarNumber)
        {
            AppointmentRequestsModel model = new AppointmentRequestsModel();

            try
            {

                DbCommand com = this.db.GetStoredProcCommand("GetPatientPersonalInfoByAadhaar");

                if (!string.IsNullOrEmpty(aadhaarNumber))
                    db.AddInParameter(com, "@AadhaarNumber", DbType.String, aadhaarNumber);
                else
                    db.AddInParameter(com, "@AadhaarNumber", DbType.String, DBNull.Value);

                DataSet ds = this.db.ExecuteDataSet(com);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];
                        model.PatientId = Convert.ToInt32(dt.Rows[0]["PatientId"]);
                        model.FirstName = Convert.ToString(dt.Rows[0]["FirstName"]);
                        model.LastName = Convert.ToString(dt.Rows[0]["LastName"]);
                        model.PatientEmail = Convert.ToString(dt.Rows[0]["Email"]);
                        model.ContactNumber = Convert.ToString(dt.Rows[0]["ContactNumber"]);
                        model.AadhaarNumber = Convert.ToString(dt.Rows[0]["AadhaarNumber"]);
                        model.DateOfBirth = Convert.ToDateTime(dt.Rows[0]["DateOfBirth"]);
                        model.Gender = Convert.ToString(dt.Rows[0]["Gender"]);

                        
                        model.AddressLine = Convert.ToString(dt.Rows[0]["AddressLine"]);
                        model.StateId = Convert.ToInt32(dt.Rows[0]["StateId"]);
                        model.DistrictId = Convert.ToInt32(dt.Rows[0]["DistrictId"]);
                        model.TalukaId = Convert.ToInt32(dt.Rows[0]["TalukaId"]);
                        model.CityId = Convert.ToInt32(dt.Rows[0]["CityId"]);
                        model.Pincode = Convert.ToString(dt.Rows[0]["Pincode"]);
                    }

            }
            catch (Exception ex)
            {
                throw;
            }

            return model;
        }

        public int SavePatientAppointment(AppointmentRequestsModel appointment)
        {
            int resultCode = -1;

            try
            {
                DbCommand cmd = db.GetStoredProcCommand("SavePatientAppointment");

                
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

                if (!string.IsNullOrEmpty(appointment.AadhaarNumber))
                    db.AddInParameter(cmd, "@AadhaarNumber", DbType.String, appointment.AadhaarNumber);
                else
                    db.AddInParameter(cmd, "@AadhaarNumber", DbType.String, DBNull.Value);

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


                
                if (appointment.UploadedFile != null && !string.IsNullOrEmpty(appointment.UploadedFile.FilePath))
                {
                    db.AddInParameter(cmd, "@DocumentFilePath", DbType.String, appointment.UploadedFile.FilePath);
                }
                else
                {
                    db.AddInParameter(cmd, "@DocumentFilePath", DbType.String, DBNull.Value);
                }


                
                db.AddOutParameter(cmd, "@AppointmentId", DbType.Int32, 4);

                db.ExecuteNonQuery(cmd);

                resultCode = Convert.ToInt32(db.GetParameterValue(cmd, "@AppointmentId"));
                return resultCode;
            }
            catch (Exception ex)
            {
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
                return -1;
            }
        }

    }


}
