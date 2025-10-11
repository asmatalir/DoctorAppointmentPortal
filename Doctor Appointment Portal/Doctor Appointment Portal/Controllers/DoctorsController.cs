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

        [HttpPost]
        public IHttpActionResult GetLists(DoctorsModel model)
        {
            try
            {
                // Call DAL to get list of doctors
                var doctors = doctorsDAL.GetList(model);
                var specializations = specializationsDAL.GetList();
                
                // Map to response model
                var response = new DoctorsModel
                {
                    TotalRecords = doctorsDAL.TotalRecords,
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


        [HttpGet]
        public IHttpActionResult AddEdit(int id)
        {
            DoctorsModel model = new DoctorsModel();
            try
            {
                //if (id != 0)
                //{
                //    // Load existing doctor details
                //    //model = doctorsDAL.LoadDoctor(id);
                //}
                //else
                //{
                //    // Initialize new doctor model
                //    model = new DoctorsModel();
                //}

                // Load dropdown lists
                model.SpecializationsList = specializationsDAL.GetList();
                model.QualificationsList = qualificationsDAL.GetList();
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
        public IHttpActionResult SaveAddEditDoctor(DoctorsModel model)
        {
            try
            {
                int doctorId = -1;

                if (model.DoctorId != 0)
                {
                    // Update existing doctor
                    //doctorId = doctorsDAL.UpdateDoctorDetails(model); // Make sure you have an Update method in DAL
                    return Ok(new { success = true, message = "Doctor updated successfully.", doctorId });
                }
                else
                {
                    // Insert new doctor
                    doctorId = doctorsDAL.SaveDoctorDetails(model); 
                    return Ok(new { success = true, message = "Doctor created successfully.", doctorId });
                }
            }
            catch (Exception ex)
            {
                // Log exception if needed
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while saving doctor details." });
            }
        }

    }
}
