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

        [HttpPost]
        public IHttpActionResult GetLists(AppointmentRequestsModel model)
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
    }
}
