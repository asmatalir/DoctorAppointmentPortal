using Doctor_Appointment_Portal.Helper;
using DoctorAppointmentPortal.Controllers;
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
    public class AccountsController : BaseApiController
    {
        Users usersDAL = new Users();
        ErrorLogs errorLogsDAL = new ErrorLogs();
        [HttpPost]
        public IHttpActionResult Login(UserProfilesModel model)
        {
            try
            {
                int userId = CurrentUserId;

                if (!usersDAL.ValidateUser(model))
                {
                    return Ok(new { success = false, message = "Username not found" });
                }
                if (!PasswordHasher.VerifyPassword(model.EnteredPassword, model.FetchedPassword))
                {
                    return Ok(new { success = false, message = "Password is incorrect" });
                }
                var token = JwtHelper.GenerateJwtToken(model);
                return Ok(new { success = true, token = token, username = model.UserName, userrole = model.RoleName });
            }
            catch (Exception ex)
            {
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while login." });
            }
        }


    }
}

