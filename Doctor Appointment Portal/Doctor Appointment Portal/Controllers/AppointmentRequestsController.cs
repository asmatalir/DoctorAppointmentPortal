using Doctor_Appointment_Portal.Helper;
using DoctorAppointmentPortal.Controllers;
using DoctorAppointmentPortalClassLibrary.DAL;
using DoctorAppointmentPortalClassLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Doctor_Appointment_Portal.Controllers
{
    public class AppointmentRequestsController : BaseApiController
    {
        AppointmentRequests appointmentRequestsDAL = new AppointmentRequests();
        Specializations specializationDAL = new Specializations();
        Statuses statusesDAL = new Statuses();
        States statesDAL = new States();
        Districts districtsDAL = new Districts();
        Talukas talukasDAL = new Talukas();
        Cities citiesDAL = new Cities();
        ErrorLogs errorLogsDAL = new ErrorLogs();

        [JwtAuthorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult AppointmentRequestsGetLists(AppointmentRequestsModel model)
        {
            try
            {
              
                var response = new AppointmentRequestsModel()
                {
                    AppointmentRequestList = appointmentRequestsDAL.GetList(model),
                    SpecializationsList = specializationDAL.GetList(),
                    StatusesList = statusesDAL.GetList(),
                    TotalRecords = appointmentRequestsDAL.TotalRecords,
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                errorLogsDAL.CurrentUserId = CurrentUserId;
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while loading appointment requests." });
            }
        }


        [JwtAuthorize(Roles = "Doctor")]
        [HttpPost]
        public IHttpActionResult DoctorApppointmentGetLists(AppointmentRequestsModel model)
        {
            try
            {

                model.DoctorId = CurrentDoctorId;
                var response = new AppointmentRequestsModel()
                {
                    AppointmentRequestList = appointmentRequestsDAL.GetDoctorAppointmentRequests(model),
                    SpecializationsList = specializationDAL.GetList(),
                    StatusesList = statusesDAL.GetList(),
                    TotalRecords = appointmentRequestsDAL.TotalRecords,
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                errorLogsDAL.CurrentUserId = CurrentUserId;
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while loading appointment requests." });
            }
        }

        [JwtAuthorize(Roles = "Doctor")]
        [HttpPost]
        public IHttpActionResult DoctorAppointmentUpdateStatus(AppointmentRequestsModel model)
        {
            try
            {

                errorLogsDAL.CurrentUserId = CurrentUserId;
                bool isUpdated = false;
                model.LastModifiedBy = CurrentUserId;
                // Reschedult appointment
                if (model.Action == "Rescheduled")
                {
                    isUpdated = appointmentRequestsDAL.RescheduleAppointment(model);
                }
                else
                {
                    // logic for Approved/Rejected etc.
                    isUpdated = appointmentRequestsDAL.UpdateAppointmentStatus(model);
                }

                if (!isUpdated)
                {
                    return Ok(new { success = false, message = "Action not allowed. Appointment already handled." });
                }


                
                string startTimeStr = model.StartTime.ToString(@"hh\:mm");
                string endTimeStr = model.EndTime.ToString(@"hh\:mm");
                string appointmentDateStr = model.PreferredDate.ToString("yyyy-MM-dd");

                //   Email to Doctor

                
                string templatesFolder = ConfigurationManager.AppSettings["EmailTemplatesFolder"];

                
                string doctorTemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, templatesFolder, "DoctorAppointmentUpdate.html");
                string patientTemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, templatesFolder, "PatientAppointmentUpdate.html");

                // Load templates
                string doctorBody = File.ReadAllText(doctorTemplatePath);
                string patientBody = File.ReadAllText(patientTemplatePath);

                // Replace placeholders for doctor
                doctorBody = doctorBody.Replace("{DoctorName}", model.DoctorName)
                                       .Replace("{Action}", model.Action)
                                       .Replace("{PatientName}", model.PatientName)
                                       .Replace("{AppointmentDate}", appointmentDateStr)
                                       .Replace("{StartTime}", startTimeStr)
                                       .Replace("{EndTime}", endTimeStr);

                string doctorSubject = $"Appointment {model.Action}";
                EmailHelper.SendEmail(model.DoctorEmail, doctorSubject, doctorBody, isHtml: true);

                // Replace placeholders for patient
                patientBody = patientBody.Replace("{PatientName}", model.PatientName)
                                         .Replace("{DoctorName}", model.DoctorName)
                                         .Replace("{Action}", model.Action)
                                         .Replace("{AppointmentDate}", appointmentDateStr)
                                         .Replace("{StartTime}", startTimeStr)
                                         .Replace("{EndTime}", endTimeStr);

                string patientSubject = $"Appointment {model.Action}";
                EmailHelper.SendEmail(model.PatientEmail, patientSubject, patientBody, isHtml: true);



                return Ok(new { success = true, message = "Appointment updated successfully." });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while updating appointment status.", error = ex.Message });
            }
        }

        [HttpGet]
        public IHttpActionResult GetPatientDetails(string aadhaarNumber)
        {
            AppointmentRequestsModel model = new AppointmentRequestsModel();
            try
            {

                 model = appointmentRequestsDAL.LoadPatientDetails(aadhaarNumber);


                return Ok(model);
            }
            catch (Exception ex)
            {
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while loading doctor details." });
            }
        }

       


        [HttpPost]
        public IHttpActionResult SavePatientAppointment()
        {
            try
            {

                var httpRequest = HttpContext.Current.Request;

                
                var jsonModel = httpRequest["model"];
                var model = JsonConvert.DeserializeObject<AppointmentRequestsModel>(jsonModel);

                // Validate the model
                ModelState.Clear();
                Validate(model);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                string uploadRootFolder = System.Configuration.ConfigurationManager.AppSettings["UploadFolder"];
                string patientFolderName = $"Patient_{model.FirstName}_{model.ContactNumber}";
                string folderPath = HttpContext.Current.Server.MapPath(Path.Combine(uploadRootFolder, patientFolderName));

                
                if (httpRequest.Files.Count > 0)
                {
                    var file = httpRequest.Files[0]; 
                    if (file != null && file.ContentLength > 0)
                    {
                        if (!Directory.Exists(folderPath))
                            Directory.CreateDirectory(folderPath);

                        string originalFileName = Path.GetFileName(file.FileName);
                        string extension = Path.GetExtension(file.FileName);
                        string uniqueFileName = $"{Guid.NewGuid()}{extension}";
                        string fullPath = Path.Combine(folderPath, uniqueFileName);

                        file.SaveAs(fullPath);

                        model.UploadedFile = new FileDetailModel
                        {
                            FileName = originalFileName,
                            FilePath = Path.Combine(patientFolderName, uniqueFileName) // store relative path
                        };
                    }
                }

                // Save appointment in DB
                int isSaved = appointmentRequestsDAL.SavePatientAppointment(model);

                if (isSaved == -2)
                {
                    if (model.UploadedFile != null)
                        File.Delete(HttpContext.Current.Server.MapPath(Path.Combine(uploadRootFolder, model.UploadedFile.FilePath)));

                    return Content(HttpStatusCode.BadRequest, new { message = "Appointment limit reached. You may schedule a maximum of two appointments on the same day." });
                }

                if (isSaved <= 0)
                {
                    if (model.UploadedFile != null)
                        File.Delete(HttpContext.Current.Server.MapPath(Path.Combine(uploadRootFolder, model.UploadedFile.FilePath)));

                    return Content(HttpStatusCode.BadRequest, new { message = "Failed to save appointment. Please try again." });
                }

                
                string startTimeStr = model.StartTime.ToString(@"hh\:mm");
                string endTimeStr = model.EndTime.ToString(@"hh\:mm");

                string templatesFolder = ConfigurationManager.AppSettings["EmailTemplatesFolder"];

                // Combine with base directory
                string doctorTemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, templatesFolder, "DoctorAppointmentRequestEmail.html");
                string patientTemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, templatesFolder, "PatientAppointmentRequestEmail.html");

                // Read the HTML template
                string doctorBody = File.ReadAllText(doctorTemplatePath);

                // Replace placeholders with actual values
                doctorBody = doctorBody.Replace("{DoctorName}", model.DoctorName)
                                       .Replace("{PatientName}", $"{model.FirstName} {model.LastName}")
                                       .Replace("{PreferredDate}", model.PreferredDate.ToString("yyyy-MM-dd"))
                                       .Replace("{StartTime}", startTimeStr)
                                       .Replace("{EndTime}", endTimeStr)
                                       .Replace("{MedicalConcern}", model.MedicalConcern);

                // Email subject
                string doctorSubject = "New Appointment Request Received";

                // Send email
                EmailHelper.SendEmail(model.DoctorEmail, doctorSubject, doctorBody, isHtml: true);
                

                
                // Read the HTML template
                string patientBody = File.ReadAllText(patientTemplatePath);

                // Replace placeholders with actual values
                patientBody = patientBody.Replace("{PatientName}", model.FirstName)
                                         .Replace("{DoctorName}", model.DoctorName)
                                         .Replace("{PreferredDate}", model.PreferredDate.ToString("yyyy-MM-dd"))
                                         .Replace("{StartTime}", startTimeStr)
                                         .Replace("{EndTime}", endTimeStr);

                // Email subject
                string patientSubject = "Appointment Request Received";

                // Send email
                EmailHelper.SendEmail(model.PatientEmail, patientSubject, patientBody, isHtml: true);


                return Ok(new { message = "Appointment saved successfully." });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while saving appointment.", error = ex.Message });
            }
        }





    }
}
