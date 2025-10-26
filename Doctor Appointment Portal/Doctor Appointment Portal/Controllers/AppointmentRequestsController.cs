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

        [JwtAuthorize]
        [HttpPost]
        public IHttpActionResult AppointmentRequestsGetLists(AppointmentRequestsModel model)
        {
            try
            {
                var appointments = appointmentRequestsDAL.GetList(model);

                // Map to response model
                var response = new AppointmentRequestsModel()
                {
                    AppointmentRequestList = appointments,
                    SpecializationsList = specializationDAL.GetList(),
                    StatusesList = statusesDAL.GetList(),
                    TotalRecords = appointmentRequestsDAL.TotalRecords,
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Return Internal Server Error with message
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while loading appointment requests." });
            }
        }
        [JwtAuthorize]
        [HttpPost]
        public IHttpActionResult DoctorApppointmentGetLists(AppointmentRequestsModel model)
        {
            try
            {

                model.DoctorId = CurrentDoctorId;
                var appointments = appointmentRequestsDAL.GetDoctorAppointmentRequests(model);
                // Map to response model
                var response = new AppointmentRequestsModel()
                {
                    AppointmentRequestList = appointments,
                    SpecializationsList = specializationDAL.GetList(),
                    StatusesList = statusesDAL.GetList(),
                    TotalRecords = appointmentRequestsDAL.TotalRecords,
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Return Internal Server Error with message
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while loading appointment requests." });
            }
        }

        [JwtAuthorize]
        [HttpPost]
        public IHttpActionResult DoctorAppointmentUpdateStatus(AppointmentRequestsModel model)
        {
            try
            {


                bool isUpdated = false;
                model.LastModifiedBy = CurrentUserId;
                // 🩺 Handle RESCHEDULE specifically
                if (model.Action == "Rescheduled")
                {
                    isUpdated = appointmentRequestsDAL.RescheduleAppointment(model);
                }
                else
                {
                    // Existing logic for Approved/Rejected/Cancelled etc.
                    isUpdated = appointmentRequestsDAL.UpdateAppointmentStatus(model);
                }

                if (!isUpdated)
                {
                    return Ok(new { success = false, message = "Action not allowed. Appointment already handled." });
                }


                // Format times for readability
                string startTimeStr = model.StartTime.ToString(@"hh\:mm");
                string endTimeStr = model.EndTime.ToString(@"hh\:mm");
                string appointmentDateStr = model.PreferredDate.ToString("yyyy-MM-dd");

                // -----------------------
                // 1️⃣  Email to Doctor
                // -----------------------
                // Path to templates
                string templatesFolder = ConfigurationManager.AppSettings["EmailTemplatesFolder"];

                // Combine with base directory
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
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IHttpActionResult GetPatientDetails(string contactNumber)
        {
            AppointmentRequestsModel model = new AppointmentRequestsModel();
            try
            {

                 model = appointmentRequestsDAL.LoadPatientDetails(contactNumber);


                return Ok(model);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while loading doctor details." });
            }
        }

        //[HttpPost]
        //public IHttpActionResult SavePatientAppointment(AppointmentRequestsModel model)
        //{
        //    try
        //    {
        //        model.CreatedBy = model.DoctorId;
        //        model.SelectedSpecializationId = 1;
        //        int isSaved = appointmentRequestsDAL.SavePatientAppointment(model);

        //        if (isSaved > 0)
        //        {
        //            string startTimeStr = model.StartTime.ToString(@"hh\:mm");
        //            string endTimeStr = model.EndTime.ToString(@"hh\:mm");
        //            string doctorSubject = "New Appointment Request Received";

        //            string doctorBody = $@"
        //                <p>Dear Dr. {model.DoctorName},</p>

        //                <p>You have received a new appointment request. Please find the details below:</p>

        //                <ul>
        //                    <li><strong>Patient Name:</strong> {model.FirstName} {model.LastName}</li>
        //                    <li><strong>Preferred Date:</strong> {model.PreferredDate:yyyy-MM-dd}</li>
        //                    <li><strong>Time:</strong> {startTimeStr} to {endTimeStr}</li>
        //                    <li><strong>Medical Concern:</strong> {model.MedicalConcern}</li>
        //                </ul>

        //                <p>Please review and approve/reject the request in your dashboard.</p>

        //                <p>Thank you,<br/>
        //                Doctor Appointment Portal Team</p>";

        //            EmailHelper.SendEmail(model.DoctorEmail, doctorSubject, doctorBody, isHtml: true);




        //            string patientSubject = "Appointment Request Received";

        //            // Build HTML email body systematically
        //            string patientBody =
        //            $"<p>Dear {model.FirstName},</p>" +
        //            $"<p>We have received your appointment request with Dr. {model.DoctorName}. Please find the details below:</p>" +
        //            $"<table style='border-collapse: collapse;'>" +
        //            $"  <tr><td style='padding: 4px;'><strong>Date:</strong></td><td style='padding: 4px;'>{model.PreferredDate:yyyy-MM-dd}</td></tr>" +
        //            $"  <tr><td style='padding: 4px;'><strong>Time:</strong></td><td style='padding: 4px;'>{startTimeStr} to {endTimeStr}</td></tr>" +
        //            $"</table>" +
        //            $"<p>You will be notified once the doctor approves or rejects the appointment.</p>" +
        //            $"<p>Thank you,<br/>Doctor Appointment Portal Team</p>";


        //            // Send email as HTML
        //            EmailHelper.SendEmail(model.PatientEmail, patientSubject, patientBody, true);

        //            return Ok(new { message = "Appointment saved successfully." });
        //        }

        //        else
        //            return Content(HttpStatusCode.BadRequest, new { message = "Failed to save appointment. Please try again." });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(HttpStatusCode.InternalServerError, new { message = "Server error while saving appointment.", error = ex.Message });
        //    }
        //}


        [HttpPost]
        public IHttpActionResult SavePatientAppointment()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;

                // Read the JSON model
                var jsonModel = httpRequest["model"];
                var model = JsonConvert.DeserializeObject<AppointmentRequestsModel>(jsonModel);

                model.SelectedSpecializationId = 1;

                string uploadRootFolder = System.Configuration.ConfigurationManager.AppSettings["UploadFolder"];
                string patientFolderName = $"Patient_{model.FirstName}_{model.ContactNumber}";
                string folderPath = HttpContext.Current.Server.MapPath(Path.Combine(uploadRootFolder, patientFolderName));

                // Handle single file (optional)
                if (httpRequest.Files.Count > 0)
                {
                    var file = httpRequest.Files[0]; // Only the first file
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
                if (isSaved <= 0)
                {
                    if (model.UploadedFile != null)
                        File.Delete(HttpContext.Current.Server.MapPath(Path.Combine(uploadRootFolder, model.UploadedFile.FilePath)));

                    return Content(HttpStatusCode.BadRequest, new { message = "Failed to save appointment. Please try again." });
                }

                // Send emails (your existing code)
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
