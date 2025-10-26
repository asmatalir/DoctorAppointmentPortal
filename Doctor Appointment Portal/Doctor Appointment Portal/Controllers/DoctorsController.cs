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

        
        [HttpPost]
        public IHttpActionResult GetLists(DoctorsModel model)
        {
            try
            {
                // Call DAL to get list of doctors
                var doctors = doctorsDAL.GetList(model);
                var specializations = specializationsDAL.GetList();
                var qualifications = qualificationsDAL.GetList();
                var cities = citiesDAL.GetList();

                // Map to response model
                var response = new DoctorsModel
                {
                    TotalRecords = doctorsDAL.TotalRecords,
                    DoctorsList = doctors,
                    SpecializationsList = specializations,
                    QualificationsList = qualifications,
                    CitiesList = cities
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Return Internal Server Error with message
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while loading doctors list." });
            }
        }

        [JwtAuthorize]
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
                    // Initialize new doctor model
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
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while loading doctor availability details." });
            }
        }


        [JwtAuthorize]
        [HttpPost]
        public IHttpActionResult SaveAddEditDoctor(DoctorsModel model)
        {
            try
            {
                int doctorId = -1;
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
                return Ok(new { success = true, message = "Doctor created successfully.", doctorId });

            }
            catch (Exception ex)
            {
                // Log exception if needed
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while saving doctor details." });
            }
        }

        [JwtAuthorize]
        [HttpPost]
        public IHttpActionResult SaveDoctorAvailability(DoctorsModel model)
        {
            try
            {
                if (model == null || model.DoctorId == 0)
                {
                    return BadRequest("DoctorId is missing or invalid.");
                }
                model.CreatedBy = CurrentUserId;
                int result = doctorsDAL.SaveDoctorAvailability(model);

                if (result > 0)
                {
                    return Ok(new { success = true, message = "Doctor availability saved successfully.", doctorId = model.DoctorId });
                }
                else
                {
                    return Content(HttpStatusCode.InternalServerError, new { success = false, message = "Failed to save doctor availability." });
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new
                {
                    success = false,
                    message = "Server error while saving doctor availability.",
                    details = ex.Message
                });
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
                return Content(HttpStatusCode.InternalServerError, new
                {
                    message = "Server error while fetching doctor slots.",
                    details = ex.Message
                });
            }


        }
    }
}
