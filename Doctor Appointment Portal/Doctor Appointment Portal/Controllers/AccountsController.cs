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
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while login." });
            }
        }

        [HttpGet]
        public IHttpActionResult Hash(string password)
        {
            if (string.IsNullOrEmpty(password))
                return BadRequest("Password is required.");

            string hashedPassword = PasswordHasher.HashPassword(password);

            return Ok(new { HashedPassword = hashedPassword });
        }

        [JwtAuthorize]
        [HttpGet]
        public IHttpActionResult TestClaims()
        {
            var userId = CurrentUserId;
            var username = CurrentUsername;
            var email = CurrentEmail;
            var role = CurrentRole;

            return Ok(new
            {
                userId,
                username,
                email,
                role
            });
        }
    }
}

