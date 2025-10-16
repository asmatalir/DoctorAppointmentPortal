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
    public class DoctorsController : ApiController
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

        [JwtAuthorize]
        [HttpPost]
        public IHttpActionResult GetLists(DoctorsModel model)
        {
            try
            {
                // Call DAL to get list of doctors
                var doctors = doctorsDAL.GetList(model);
                var specializations = specializationsDAL.GetList();
                var qualifications = qualificationsDAL.GetList();

                // Map to response model
                var response = new DoctorsModel
                {
                    TotalRecords = doctorsDAL.TotalRecords,
                    DoctorsList = doctors,
                    SpecializationsList = specializations,
                    QualificationsList = qualifications
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Return Internal Server Error with message
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while loading doctors list." });
            }
        }


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
                model.StatesList = statesDAL.GetList();
                model.DistrictsList = districtsDAL.GetList();
                model.TalukasList = talukasDAL.GetList();
                model.CitiesList = citiesDAL.GetList();
                model.DoctorAvailabilityList = doctorAvailabilitiesDAL.GetDetails(id);
                model.DoctorAvailabilityExceptionsList = doctorAvailabilityExceptionsDAL.GetDetails(id);


                return Ok(model);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while loading doctor details." });
            }
        }

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



        [HttpPost]
        public IHttpActionResult SaveAddEditDoctor(DoctorsModel model)
        {
            try
            {
                int doctorId = -1;

                // Insert new doctor
                doctorId = doctorsDAL.SaveDoctorDetails(model);
                return Ok(new { success = true, message = "Doctor created successfully.", doctorId });

            }
            catch (Exception ex)
            {
                // Log exception if needed
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while saving doctor details." });
            }
        }

        [HttpPost]
        public IHttpActionResult SaveDoctorAvailability(DoctorsModel model)
        {
            try
            {
                if (model == null || model.DoctorId == 0)
                {
                    return BadRequest("DoctorId is missing or invalid.");
                }

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
