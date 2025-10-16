using DoctorAppointmentPortalClassLibrary.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace Doctor_Appointment_Portal.Helper
{
    public class JwtHelper
    {
        public static string GenerateJwtToken(UserProfilesModel model)
        {
            var key = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["JwtSecret"]);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, model.UserId.ToString()), 
                new Claim(ClaimTypes.Name, model.UserName),                    
                new Claim(ClaimTypes.Email, model.Email ?? string.Empty),       
                new Claim(ClaimTypes.Role, model.RoleName ?? string.Empty),         
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}