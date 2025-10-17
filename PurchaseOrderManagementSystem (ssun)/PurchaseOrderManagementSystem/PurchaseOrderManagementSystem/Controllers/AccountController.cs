using Microsoft.IdentityModel.Tokens;
using PurchaseOrderManagementSystem.Helper;
using PurchaseOrderManagementSystemClassLibrary.DAL;
using PurchaseOrderManagementSystemClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PurchaseOrderManagementSystem.Controllers
{
    public class AccountController : BaseController
    {
        Users userDAL = new Users();
        ErrorLogs errorLogsDAL = new ErrorLogs();
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserInfoModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Where(x => x.Value.Errors.Count > 0).ToDictionary( kvp => kvp.Key,  kvp => kvp.Value.Errors.First().ErrorMessage );
                    return Json(new { success = false, errors });
                }
                errorLogsDAL.CurrentUserId = CurrentUserId;

                //Primary Password is retrieved from DB
                if (userDAL.GetUserByUserName(model))
                {
                    //Entered Primary Password and Stored Primary Password are Compared
                    if(PasswordHasher.VerifyPassword(model.EnteredPrimaryPassword, model.PrimaryPassword))
                    {
                        //Alphanumeric SecondaryPassword is Generated
                        string otp = OtpHelper.GenerateAlphanumericOtp();
                        //SecondaryPassword is Encrypted
                        string encryptedOtp = CryptoEncryption.Encrypt(otp);
                        //Updates the SecondaryPassword in DB
                        userDAL.UpdateSecondaryPassword(model.UserName, encryptedOtp);
                        //Sends SecondaryPassword as Email to user
                        EmailHelper.SendSecondaryPassword(model.Email, otp);
                        return Json(new { success = true, userName = model.UserName });
                    }
                    return Json(new { success = false, error = "Invalid password." });
                }
                return Json(new { success = false, error = "Incorrect Username" });
            }
            catch(Exception ex)
            {
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
                return Json(new { success = false, message = "An error occurred. Please try again." });
            }
        }

        [HttpPost]
        public JsonResult VerifySecondaryPassword(UserInfoModel model)
        {
            try
            {
                //Retrieve the SecondaryPassword from DB
                var result = userDAL.GetSecondaryPasswordByUserName(model.UserName);
                model.SecondaryPassword = result.SecondaryPassword;
                model.UserId = result.UserId;
                errorLogsDAL.CurrentUserId = CurrentUserId;

                if (!string.IsNullOrEmpty(model.SecondaryPassword))
                {
                    //Decrypt the SecondaryPassword
                    string decryptedOtp = CryptoEncryption.Decrypt(model.SecondaryPassword);

                    //Entered SeondaryPassword and Retreived Password is compared
                    if (model.EnteredSecondaryPassword == decryptedOtp)
                    {
                        //Generate Jwt Token
                        var token = JwtHelper.GenerateJwtToken(model);
                        //Store the Jwt Token in cookie
                        var cookie = new HttpCookie("jwtToken", token) { HttpOnly = true };
                        Response.Cookies.Add(cookie);

                        return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });
                    }

                    return Json(new { success = false, error = "Invalid OTP." });
                }

                return Json(new { success = false, error = "OTP not found." });
            }
            catch(Exception ex)
            {
                errorLogsDAL.InsertErrorLogs(ex.Message, ex.StackTrace);
                return Json(new { success = false, message = "An error occurred. Please try again." });
            }
         }

        public ActionResult Logout()
        {
            if (Request.Cookies["jwtToken"] != null)
            {
                var cookie = new HttpCookie("jwtToken")
                {
                    Expires = DateTime.Now.AddDays(-1), 
                    HttpOnly = true                     
                };
                Response.Cookies.Add(cookie);           
            }

            return RedirectToAction("Login", "Account"); 
        }


    }
}