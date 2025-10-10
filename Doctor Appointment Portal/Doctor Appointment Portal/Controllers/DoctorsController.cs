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

        [HttpGet]
        public IHttpActionResult GetLists()
        {
            try
            {
                // Call DAL to get list of doctors
                var doctors = doctorsDAL.GetList();
                var specializations = specializationsDAL.GetList();

                // Map to response model
                var response = new DoctorsModel
                {
                    DoctorsList = doctors,
                    SpecializationsList = specializations
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Return Internal Server Error with message
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while loading doctors list." });
            }
        }
    }
}
