using Doctor_Appointment_Portal.Helper;
using DoctorAppointmentPortalClassLibrary.DAL;
using DoctorAppointmentPortalClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Doctor_Appointment_Portal.Controllers
{
    public class AppointmentRequestsController : ApiController
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

        [HttpPost]
        public IHttpActionResult DoctorApppointmentGetLists(AppointmentRequestsModel model)
        {
            try
            {
                model.DoctorId = 14;
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


                bool isUpdated = appointmentRequestsDAL.UpdateAppointmentStatus(model);

                if (!isUpdated)
                {
                    return BadRequest("Failed to update appointment status.");
                }

                model.DoctorEmail = "asmataliravtar@gmail.com";
                // 3. Send email to doctor
                string doctorSubject = $"Appointment {model.StatusName}";
                string doctorBody = $"Appointment with {model.PatientName} on {model.FinalDate:yyyy-MM-dd} " +
                                    $"from {model.FinalStartTime:hh\\:mm} to {model.FinalEndTime:hh\\:mm} " +
                                    $"has been {model.Action}.";
                EmailHelper.SendEmail(model.DoctorEmail, doctorSubject, doctorBody);

                // 4. Send email to patient
                string patientSubject = $"Appointment {model.StatusName}";
                string patientBody = $"Your appointment with Dr. {model.DoctorName} on {model.FinalDate:yyyy-MM-dd}" +
                                     $"from {model.FinalStartTime:hh\\:mm} to {model.FinalEndTime:hh\\:mm} " + 
                                     $"has been {model.Action}.";
                EmailHelper.SendEmail(model.PatientEmail, patientSubject, patientBody);

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

        [HttpPost]
        public IHttpActionResult SavePatientAppointment(AppointmentRequestsModel model)
        {
            try
            {
                model.CreatedBy = 6;
                int isSaved = appointmentRequestsDAL.SavePatientAppointment(model);

                if (isSaved > 0)
                    return Ok(new { message = "Appointment saved successfully." });
                else
                    return Content(HttpStatusCode.BadRequest, new { message = "Failed to save appointment. Please try again." });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while saving appointment.", error = ex.Message });
            }
        }




    }
}
