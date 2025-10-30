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
    public class LocationController : ApiController
    {
        States statesDAL = new States();
        Districts districtsDAL = new Districts();
        Talukas talukasDAL = new Talukas();
        Cities citiesDAL = new Cities();

        [HttpGet]
        public IHttpActionResult GetStates()
        {
            try
            {
                var result = statesDAL.GetList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while loading states." });
            }
        }

        [HttpGet]
        public IHttpActionResult GetDistricts(int stateId)
        {
            try
            {
                var result = districtsDAL.GetDistrictsByState(stateId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while loading districts." });
            }
        }

        [HttpGet]
        public IHttpActionResult GetTalukas(int districtId)
        {
            try
            {
                var result = talukasDAL.GetTalukasByDistrict(districtId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while loading talukas." });
            }
        }


        [HttpGet]
        public IHttpActionResult GetCities(int talukaId)
        {
            try
            {
                var result = citiesDAL.GetCitiesByTaluka(talukaId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while loading cities." });
            }
        }
    }
}

