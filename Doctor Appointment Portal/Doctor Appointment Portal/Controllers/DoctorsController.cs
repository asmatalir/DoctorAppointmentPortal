using DoctorAppointmentPortalClassLibrary.DAL;
using DoctorAppointmentPortalClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Doctor_Appointment_Portal.Helper;
using DoctorAppointmentPortal.Controllers;

namespace Doctor_Appointment_Portal.Controllers
{
    public class DoctorsController : BaseApiController
    {
        Doctors doctorsDAL = new Doctors();
        Specializations specializationsDAL = new Specializations();
        Qualifications qualificationsDAL = new Qualifications();
        States statesDAL = new States();
        Districts districtsDAL = new Districts();
        Talukas talukasDAL = new Talukas();
        Cities citiesDAL = new Cities();
        DoctorAvailabilities doctorAvailabilitiesDAL = new DoctorAvailabilities();
        DoctorAvailabilityExceptions doctorAvailabilityExceptionsDAL = new DoctorAvailabilityExceptions();
        DoctorAvailableSlots doctorSlotsDAL = new DoctorAvailableSlots();
        ErrorLogs errorLogsDAL = new ErrorLogs();

        
        [HttpPost]
        public IHttpActionResult GetLists(DoctorsModel model)
        {
            try
            {
                
                var response = new DoctorsModel
                {                   
                    DoctorsList = doctorsDAL.GetList(model),
                    SpecializationsList = specializationsDAL.GetList(),
                    QualificationsList = qualificationsDAL.GetList(),
                    CitiesList = citiesDAL.GetList(),
                    TotalRecords = doctorsDAL.TotalRecords,
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                errorLogsDAL.CurrentUserId = CurrentUserId;
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while loading doctors list." });
            }
        }

        [JwtAuthorize(Roles = "Admin")]
        [HttpGet]
        public IHttpActionResult GetDoctorDetails(int id)
        {
            DoctorsModel model = new DoctorsModel();
            try
            {
                if (id != 0)
                {

                    model = doctorsDAL.LoadDoctorDetails(id);
                }
                else
                {
                    
                    model = new DoctorsModel();
                }

                // Load dropdown lists
                model.SpecializationsList = specializationsDAL.GetList();
                model.QualificationsList = qualificationsDAL.GetList();
                model.DoctorAvailabilityList = doctorAvailabilitiesDAL.GetDetails(id);
                model.DoctorAvailabilityExceptionsList = doctorAvailabilityExceptionsDAL.GetDetails(id);


                return Ok(model);
            }
            catch (Exception ex)
            {
                errorLogsDAL.CurrentUserId = CurrentUserId;
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while loading doctor details." });
            }
        }

        [JwtAuthorize]
        [HttpGet]
        public IHttpActionResult GetDoctorAvailabilityDetails(int id)
        {
            DoctorsModel model = new DoctorsModel();
            try
            {

                model = doctorsDAL.LoadDoctorDetails(id);
                model.DoctorAvailabilityList = doctorAvailabilitiesDAL.GetDetails(id);
                model.DoctorAvailabilityExceptionsList = doctorAvailabilityExceptionsDAL.GetDetails(id);


                return Ok(model);
            }
            catch (Exception ex)
            {
                errorLogsDAL.CurrentUserId = CurrentUserId;
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while loading doctor availability details." });
            }
        }


        [JwtAuthorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult SaveAddEditDoctor(DoctorsModel model)
        {
            try
            {

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                int doctorId = -1;
                errorLogsDAL.CurrentUserId = CurrentUserId;
                model.CreatedBy = CurrentUserId;
                if (!string.IsNullOrEmpty(model.HashedPassword))
                {
                    model.HashedPassword = PasswordHasher.HashPassword(model.HashedPassword);
                }
                else
                {
                    
                    model.HashedPassword = null;
                }
                doctorId = doctorsDAL.SaveDoctorDetails(model);
                if (doctorId > 0)
                {
                    return Ok(new { success = true, message = "Doctor created successfully." });
                }
                else
                {
                    return Ok(new { success = false, message = "Failed to save doctor details. Please try again." });
                }

            }
            catch (Exception ex)
            {
                
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while saving doctor details." });
            }
        }

        [JwtAuthorize]
        [HttpPost]
        public IHttpActionResult SaveDoctorAvailability(DoctorsModel model)
        {
            try
            {
                if (model.DoctorId == 0)
                {
                    return BadRequest("Doctor details not found.");
                }
                model.CreatedBy = CurrentUserId;
                errorLogsDAL.CurrentUserId = CurrentUserId;
                int result = doctorsDAL.SaveDoctorAvailability(model);

                if (result > 0)
                {
                    return Ok(new { success = true, message = "Doctor availability saved successfully." });
                }
                else
                {
                    return Content(HttpStatusCode.InternalServerError, new { success = false, message = "Failed to save doctor availability." });
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new{ success = false, message = "Server error while saving doctor availability.", details = ex.Message });
            }
        }


        [HttpGet]
        public IHttpActionResult GetDoctorSlots(int id)
        {
            try
            {
                List<DoctorAvailableSlotsModel> slots = doctorSlotsDAL.GetSlots(id);

                return Ok(slots);
            }
            catch (Exception ex)
            {
                errorLogsDAL.CurrentUserId = CurrentUserId;
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while fetching doctor slots.", details = ex.Message });
            }


        }
    }
}
