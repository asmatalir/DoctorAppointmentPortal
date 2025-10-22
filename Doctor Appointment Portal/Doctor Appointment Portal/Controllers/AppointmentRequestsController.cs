using Doctor_Appointment_Portal.Helper;
using DoctorAppointmentPortal.Controllers;
using DoctorAppointmentPortalClassLibrary.DAL;
using DoctorAppointmentPortalClassLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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


        [HttpPost]
        public IHttpActionResult DoctorAppointmentUpdateStatus(AppointmentRequestsModel model)
        {
            try
            {


                bool isUpdated = false;

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
                    return BadRequest("Failed to update appointment status.");
                }


                // Format times for readability
                string startTimeStr = model.StartTime.ToString(@"hh\:mm");
                string endTimeStr = model.EndTime.ToString(@"hh\:mm");
                string appointmentDateStr = model.PreferredDate.ToString("yyyy-MM-dd");

                // -----------------------
                // 1️⃣  Email to Doctor
                // -----------------------
                string doctorSubject = $"Appointment {model.Action}";

                string doctorBody = $@"
                    <p>Dear Dr. {model.DoctorName},</p>

                    <p>The following appointment has been <strong>{model.Action}</strong>. Please find the updated details below:</p>

                    <table style='border-collapse: collapse;'>
                        <tr>
                            <td style='padding: 4px;'><strong>Patient Name:</strong></td>
                            <td style='padding: 4px;'>{model.PatientName}</td>
                        </tr>
                        <tr>
                            <td style='padding: 4px;'><strong>New Date:</strong></td>
                            <td style='padding: 4px;'>{appointmentDateStr}</td>
                        </tr>
                        <tr>
                            <td style='padding: 4px;'><strong>New Time:</strong></td>
                            <td style='padding: 4px;'>{startTimeStr} to {endTimeStr}</td>
                        </tr>
                    </table>

                    <p>Please review your updated schedule in your dashboard.</p>

                    <p>Thank you,<br/>
                    <strong>Doctor Appointment Portal Team</strong></p>";

                EmailHelper.SendEmail(model.DoctorEmail, doctorSubject, doctorBody, isHtml: true);


                // -----------------------
                // 2️⃣  Email to Patient
                // -----------------------
                string patientSubject = $"Appointment {model.Action}";

                string patientBody = $@"
                    <p>Dear {model.PatientName},</p>

                    <p>Your appointment with <strong>Dr. {model.DoctorName}</strong> has been <strong>{model.Action}</strong>. Please find the updated details below:</p>

                    <table style='border-collapse: collapse;'>
                        <tr>
                            <td style='padding: 4px;'><strong>Date:</strong></td>
                            <td style='padding: 4px;'>{appointmentDateStr}</td>
                        </tr>
                        <tr>
                            <td style='padding: 4px;'><strong>Time:</strong></td>
                            <td style='padding: 4px;'>{startTimeStr} to {endTimeStr}</td>
                        </tr>
                    </table>

                    <p>Please check your portal for the latest appointment status.</p>

                    <p>Thank you,<br/>
                    <strong>Doctor Appointment Portal Team</strong></p>";

                EmailHelper.SendEmail(model.PatientEmail, patientSubject, patientBody, isHtml: true);


                return Ok(new { success = true });
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

                // Load dropdown lists

                model.StatesList = statesDAL.GetList();
                model.DistrictsList = districtsDAL.GetList();
                model.TalukasList = talukasDAL.GetList();
                model.CitiesList = citiesDAL.GetList();


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

                model.CreatedBy = model.DoctorId;
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

                string doctorSubject = "New Appointment Request Received";
                string doctorBody = $@"
            <p>Dear Dr. {model.DoctorName},</p>
            <p>You have received a new appointment request. Please find the details below:</p>
            <ul>
                <li><strong>Patient Name:</strong> {model.FirstName} {model.LastName}</li>
                <li><strong>Preferred Date:</strong> {model.PreferredDate:yyyy-MM-dd}</li>
                <li><strong>Time:</strong> {startTimeStr} to {endTimeStr}</li>
                <li><strong>Medical Concern:</strong> {model.MedicalConcern}</li>
            </ul>
            <p>Please review and approve/reject the request in your dashboard.</p>
            <p>Thank you,<br/>Doctor Appointment Portal Team</p>";

                EmailHelper.SendEmail(model.DoctorEmail, doctorSubject, doctorBody, isHtml: true);

                string patientSubject = "Appointment Request Received";
                string patientBody =
                    $"<p>Dear {model.FirstName},</p>" +
                    $"<p>We have received your appointment request with Dr. {model.DoctorName}. Please find the details below:</p>" +
                    $"<table style='border-collapse: collapse;'>" +
                    $"  <tr><td style='padding: 4px;'><strong>Date:</strong></td><td style='padding: 4px;'>{model.PreferredDate:yyyy-MM-dd}</td></tr>" +
                    $"  <tr><td style='padding: 4px;'><strong>Time:</strong></td><td style='padding: 4px;'>{startTimeStr} to {endTimeStr}</td></tr>" +
                    $"</table>" +
                    $"<p>You will be notified once the doctor approves or rejects the appointment.</p>" +
                    $"<p>Thank you,<br/>Doctor Appointment Portal Team</p>";

                EmailHelper.SendEmail(model.PatientEmail, patientSubject, patientBody, true);

                return Ok(new { message = "Appointment saved successfully." });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while saving appointment.", error = ex.Message });
            }
        }





    }
}
