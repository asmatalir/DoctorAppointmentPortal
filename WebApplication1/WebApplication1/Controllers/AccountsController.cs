using LibraryManagementSystemClassLibrary.DAL;
using LibraryManagementSystemClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Helpers;

namespace WebApplication1.Controllers
{
    
    public class AccountsController : ApiController
    {
        Users usersDAL = new Users();

        [HttpPost]
        public IHttpActionResult Login(UserInfoModel model)
        {
            try
            {
                int userId = 1;

                if (!usersDAL.GetUserByUserName(model))
                {
                    return Ok(new { success = false, message = "Username not found" });
                }
                if (!PasswordHasher.VerifyPassword(model.EnteredPassword, model.Password))
                {
                    return Ok(new { success = false, message = "Password is incorrect" });
                }
                var token = JwtHelper.GenerateJwtToken(model.UserName, userId,model.Role);
                return Ok(new { success = true, token = token, username = model.UserName,userrole = model.Role });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { message = "Server error while login." });
            }
        }

        public IHttpActionResult VerifyEmail(PasswordResetModel model)
        {
            try
            {
                bool userExists = usersDAL.VerifyEmail(model.Email);
                if(userExists)
                {
                    model.Token = Guid.NewGuid().ToString(); 
                    model.Expiry = DateTime.UtcNow.AddHours(1);
                    model.UserId = 1;
                    usersDAL.SavePasswordResetToken(model); 

                    string resetLink = $"http://localhost:4200/auth/pass-create/basic?token={model.Token}";
                    string subject = "Reset your password";
                    string body = $"Hi,\n\nClick the link below to reset your password (valid for 1 hour):\n{resetLink}\n\nIf you didn't request this, ignore this email.";

                    EmailHelper.SendEmail(model.Email, subject, body);
                }
                return Ok(new { message = "If this email is registered, a reset link has been sent." });
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        public IHttpActionResult ResetPassword(PasswordResetModel model)
        {
            try
            {
                model.UserId = 1;
                var tokenFetched = usersDAL.GetResetPasswordTokenDetails(model);

                if (!tokenFetched)
                    return Ok(new { success = false, message = "No reset token found for this user." });

                if (model.Token != model.token)
                    return Ok(new { success = false, message = "Invalid link." });

                if (model.IsUsed == 1)
                    return Ok(new { success = false, message = "This reset link has already been used." });


                if (DateTime.UtcNow > model.Expiry)
                    return Ok(new { success = false, message = "This reset link has expired." });

                string hashedPassword = PasswordHasher.HashPassword(model.Password);
                model.Password = hashedPassword;

                // 4. Update password and mark token as used
                var updated = usersDAL.UpdatePassword(model);

                if (!updated)
                    return Ok(new { success = false, message = "Server Error." });


                return Ok(new { success = true, message = "Password reset successfully." });
            }
            catch (Exception ex)
            {
                // errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
                return InternalServerError();
            }
        }

        [HttpGet]
        public IHttpActionResult ValidateToken()
        {
            return Ok(new { valid = true });
        }

        [HttpGet]
        public IHttpActionResult PasswordGenerator(string password)
        {

            var hashedPassword = PasswordHasher.HashPassword(password);

            return Ok(new { hashedPassword, valid = true });
        }



    }
}

