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
        public int TotalRecords { get; set; }
        //ErrorLogs errorLogsDAL = new ErrorLogs();
        public List<DoctorsModel> GetList(DoctorsModel model)
        {

            List<DoctorsModel> DoctorsList = new List<DoctorsModel>();
            try
            {
                DbCommand com = db.GetStoredProcCommand("DoctorsGetList");
                if (!string.IsNullOrEmpty(model.SearchedDoctorName))
                {
                    db.AddInParameter(com, "@DoctorName", DbType.String, model.SearchedDoctorName);
                }
                else
                {
                    db.AddInParameter(com, "@DoctorName", DbType.String, DBNull.Value);
                }

                if (model.SelectedSpecializationId >0 )
                {
                    db.AddInParameter(com, "@SpecializationId", DbType.String, model.SelectedSpecializationId);
                }
                else
                {
                    db.AddInParameter(com, "@SpecializationId", DbType.String, DBNull.Value);
                }
                if (model.SelectedCity > 0)
                {
                    db.AddInParameter(com, "@CityId", DbType.String, model.SelectedCity);
                }
                else
                {
                    db.AddInParameter(com, "@CityId", DbType.String, DBNull.Value);
                }
                db.AddInParameter(com, "@PageNumber", DbType.Int32, model.PageNumber);
                db.AddInParameter(com, "@PageSize", DbType.Int32, model.PageSize);
                db.AddOutParameter(com, "@TotalCount", DbType.Int32, sizeof(int));

                DataSet ds = db.ExecuteDataSet(com);
                this.TotalRecords = Convert.ToInt32(db.GetParameterValue(com, "@TotalCount"));
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
                            IsActive = Convert.ToBoolean(row["IsActive"]),
                            SpecializationNames = row.Table.Columns.Contains("SpecializationNames")? Convert.ToString(row["SpecializationNames"]): string.Empty
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

        public DoctorsModel LoadDoctorDetails(int doctorId)
        {
            DoctorsModel model = new DoctorsModel();

            try
            {
                if (doctorId != 0)
                {
                    DbCommand com = this.db.GetStoredProcCommand("GetDoctorDetailsById");
                    db.AddInParameter(com, "DoctorId", DbType.Int32, doctorId);

                    DataSet ds = this.db.ExecuteDataSet(com);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];
                        model.DoctorId = Convert.ToInt32(dt.Rows[0]["DoctorId"]);
                        model.FirstName = Convert.ToString(dt.Rows[0]["FirstName"]);
                        model.LastName = Convert.ToString(dt.Rows[0]["LastName"]);
                        model.Email = Convert.ToString(dt.Rows[0]["Email"]);
                        model.ContactNumber = Convert.ToString(dt.Rows[0]["ContactNumber"]);
                        model.Gender = Convert.ToString(dt.Rows[0]["Gender"]);
                        model.ExperienceYears = Convert.ToInt32(dt.Rows[0]["ExperienceYears"]);
                        model.ConsultationFees = Convert.ToDecimal(dt.Rows[0]["ConsultationFees"]);
                        model.HospitalName = Convert.ToString(dt.Rows[0]["HospitalName"]);
                        model.Description = Convert.ToString(dt.Rows[0]["Description"]);
                        model.Rating = Convert.ToDecimal(dt.Rows[0]["Rating"]);

                        model.AddressId = Convert.ToInt32(dt.Rows[0]["AddressId"]);
                        model.StateId = Convert.ToInt32(dt.Rows[0]["StateId"]);
                        model.DistrictId = Convert.ToInt32(dt.Rows[0]["DistrictId"]);
                        model.TalukaId = Convert.ToInt32(dt.Rows[0]["TalukaId"]);
                        model.CityId = Convert.ToInt32(dt.Rows[0]["CityId"]);

                        model.SpecializationIds = Convert.ToString(dt.Rows[0]["SpecializationIds"]);
                        model.SpecializationNames = Convert.ToString(dt.Rows[0]["SpecializationNames"]);
                        model.QualificationIds = Convert.ToString(dt.Rows[0]["QualificationIds"]);
                        model.QualificationNames = Convert.ToString(dt.Rows[0]["QualificationNames"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoadDoctorDetails: {ex.Message}");
                // Handle Exception
            }

            return model;
        }


        public int SaveDoctorDetails(DoctorsModel doctor)
        {
            int resultCode = -1;


            try
            {


                DbCommand cmd = db.GetStoredProcCommand("InsertDoctorDetails");

                // Personal Info
                if (!string.IsNullOrEmpty(doctor.FirstName))
                    db.AddInParameter(cmd, "@FirstName", DbType.String, doctor.FirstName);
                else
                    db.AddInParameter(cmd, "@FirstName", DbType.String, DBNull.Value);

                if (!string.IsNullOrEmpty(doctor.LastName))
                    db.AddInParameter(cmd, "@LastName", DbType.String, doctor.LastName);
                else
                    db.AddInParameter(cmd, "@LastName", DbType.String, DBNull.Value);

                if (!string.IsNullOrEmpty(doctor.Email))
                    db.AddInParameter(cmd, "@Email", DbType.String, doctor.Email);
                else
                    db.AddInParameter(cmd, "@Email", DbType.String, DBNull.Value);

                if (!string.IsNullOrEmpty(doctor.ContactNumber))
                    db.AddInParameter(cmd, "@ContactNumber", DbType.String, doctor.ContactNumber);
                else
                    db.AddInParameter(cmd, "@ContactNumber", DbType.String, DBNull.Value);

                if (!string.IsNullOrEmpty(doctor.UserName))
                    db.AddInParameter(cmd, "@UserName", DbType.String, doctor.UserName);
                else
                    db.AddInParameter(cmd, "@UserName", DbType.String, DBNull.Value);

                if (!string.IsNullOrEmpty(doctor.HashedPassword))
                    db.AddInParameter(cmd, "@HashedPassword", DbType.String, doctor.HashedPassword);
                else
                    db.AddInParameter(cmd, "@HashedPassword", DbType.String, DBNull.Value);

                if (!string.IsNullOrEmpty(doctor.Gender))
                    db.AddInParameter(cmd, "@Gender", DbType.String, doctor.Gender);
                else
                    db.AddInParameter(cmd, "@Gender", DbType.String, DBNull.Value);

                if (doctor.DateOfBirth != DateTime.MinValue)
                    db.AddInParameter(cmd, "@DateOfBirth", DbType.Date, doctor.DateOfBirth);
                else
                    db.AddInParameter(cmd, "@DateOfBirth", DbType.Date, DBNull.Value);

                db.AddInParameter(cmd, "@CreatedBy", DbType.Int32, 1);

                // Professional Info
                if (doctor.ExperienceYears > 0)
                    db.AddInParameter(cmd, "@ExperienceYears", DbType.Int32, doctor.ExperienceYears);
                else
                    db.AddInParameter(cmd, "@ExperienceYears", DbType.Int32, DBNull.Value);

                if (doctor.ConsultationFees > 0)
                    db.AddInParameter(cmd, "@ConsultationFees", DbType.Decimal, doctor.ConsultationFees);
                else
                    db.AddInParameter(cmd, "@ConsultationFees", DbType.Decimal, DBNull.Value);

                if (!string.IsNullOrEmpty(doctor.Description))
                    db.AddInParameter(cmd, "@Description", DbType.String, doctor.Description);
                else
                    db.AddInParameter(cmd, "@Description", DbType.String, DBNull.Value);

                if (!string.IsNullOrEmpty(doctor.HospitalName))
                    db.AddInParameter(cmd, "@HospitalName", DbType.String, doctor.HospitalName);
                else
                    db.AddInParameter(cmd, "@HospitalName", DbType.String, DBNull.Value);

                if (doctor.Rating != null)
                    db.AddInParameter(cmd, "@Rating", DbType.Decimal, doctor.Rating);
                else
                    db.AddInParameter(cmd, "@Rating", DbType.Decimal, DBNull.Value);

                // Address Info
                if (!string.IsNullOrEmpty(doctor.AddressLine))
                    db.AddInParameter(cmd, "@AddressLine", DbType.String, doctor.AddressLine);
                else
                    db.AddInParameter(cmd, "@AddressLine", DbType.String, DBNull.Value);

                if (doctor.StateId > 0)
                    db.AddInParameter(cmd, "@StateId", DbType.Int32, doctor.StateId);
                else
                    db.AddInParameter(cmd, "@StateId", DbType.Int32, DBNull.Value);

                if (doctor.DistrictId > 0)
                    db.AddInParameter(cmd, "@DistrictId", DbType.Int32, doctor.DistrictId);
                else
                    db.AddInParameter(cmd, "@DistrictId", DbType.Int32, DBNull.Value);

                if (doctor.TalukaId > 0)
                    db.AddInParameter(cmd, "@TalukaId", DbType.Int32, doctor.TalukaId);
                else
                    db.AddInParameter(cmd, "@TalukaId", DbType.Int32, DBNull.Value);

                if (doctor.CityId != null && doctor.CityId > 0)
                    db.AddInParameter(cmd, "@CityId", DbType.Int32, doctor.CityId);
                else
                    db.AddInParameter(cmd, "@CityId", DbType.Int32, DBNull.Value);

                if (!string.IsNullOrEmpty(doctor.Pincode))
                    db.AddInParameter(cmd, "@Pincode", DbType.String, doctor.Pincode);
                else
                    db.AddInParameter(cmd, "@Pincode", DbType.String, DBNull.Value);

 

                if (!string.IsNullOrEmpty(doctor.QualificationIds))
                    db.AddInParameter(cmd, "@Qualifications", DbType.String, doctor.QualificationIds);
                else
                    db.AddInParameter(cmd, "@Qualifications", DbType.String, DBNull.Value);

                if (!string.IsNullOrEmpty(doctor.SpecializationIds))
                    db.AddInParameter(cmd, "@Specializations", DbType.String, doctor.SpecializationIds);
                else
                    db.AddInParameter(cmd, "@Specializations", DbType.String, DBNull.Value);

                // Output DoctorId
                db.AddOutParameter(cmd, "@CurrentDoctorId", DbType.Int32, 4);

                db.ExecuteNonQuery(cmd);

                resultCode = Convert.ToInt32(db.GetParameterValue(cmd, "@CurrentDoctorId"));
                return resultCode;
            }
            catch (Exception ex)
            {


                //errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace, CreatedBy);
                return -1;
            }
        }

        public int SaveDoctorAvailability(DoctorsModel doctor)
        {
            int resultCode = 1;
            try
            {

                DbCommand cmd = db.GetStoredProcCommand("InsertOrUpdateDoctorAvailability");
                if(doctor.DoctorId > 0)
                     db.AddInParameter(cmd, "@DoctorId", DbType.Int32, doctor.DoctorId);
                else
                    db.AddInParameter(cmd, "@DoctorId", DbType.Int32, DBNull.Value);


                db.AddInParameter(cmd, "@CreatedBy", DbType.Int32, 6);

                // Convert availability list to XML
                string availabilitiesXml = ConvertAvailabilitiesToXml(doctor.DoctorAvailabilityList);
                if (!String.IsNullOrEmpty(availabilitiesXml))
                {
                    db.AddInParameter(cmd, "@AvailabilitiesXml", DbType.Xml, availabilitiesXml);
                }
                else
                {
                    db.AddInParameter(cmd, "@AvailabilitiesXml", DbType.Xml, DBNull.Value);
                }

                db.AddOutParameter(cmd, "@ResultCode", DbType.Int32, 4);

                // Execute
                db.ExecuteNonQuery(cmd);

                resultCode = Convert.ToInt32(db.GetParameterValue(cmd, "@ResultCode"));
                return resultCode;
            }
            catch (Exception ex)
            {

                //errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace, doctor.CreatedBy);
                return -1;
            }
        }



        private string ConvertAvailabilitiesToXml(List<DoctorAvailabilitiesModel> availabilities)
        {
            if (availabilities == null || availabilities.Count == 0)
                return "<Availabilities></Availabilities>";

            var sb = new StringBuilder();
            sb.Append("<Availabilities>");
            foreach (var slot in availabilities)
            {
                sb.Append("<Availability>");
                sb.AppendFormat("<DayOfWeek>{0}</DayOfWeek>", slot.DayOfWeek);
                sb.AppendFormat("<StartTime>{0}</StartTime>", slot.StartTime);
                sb.AppendFormat("<EndTime>{0}</EndTime>", slot.EndTime);
                sb.AppendFormat("<SlotDuration>{0}</SlotDuration>", slot.Duration);
                sb.Append("</Availability>");
            }
            sb.Append("</Availabilities>");
            return sb.ToString();
        }



    }
}
