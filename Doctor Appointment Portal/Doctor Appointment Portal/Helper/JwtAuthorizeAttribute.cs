using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;

public class JwtAuthorizeAttribute : AuthorizeAttribute
{
    private static readonly SymmetricSecurityKey SigningKey =
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["JwtSecret"]));

    protected override bool IsAuthorized(HttpActionContext actionContext)
    {
        var authHeader = actionContext.Request.Headers.Authorization;
        string token = null;

        if (authHeader != null && authHeader.Scheme == "Bearer")
        {
            token = authHeader.Parameter;
        }

        if (string.IsNullOrEmpty(token)) return false;





        var tokenHandler = new JwtSecurityTokenHandler();
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
        try
        {
            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                IssuerSigningKey = SigningKey
            }, out validatedToken);


            actionContext.RequestContext.Principal = principal;

            if (!string.IsNullOrEmpty(Roles))
            {
                var requiredRoles = Roles.Split(',').Select(r => r.Trim());
                var userRoles = principal.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
                if (!requiredRoles.Any(r => userRoles.Contains(r)))
                {
                    return false;
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Token validation failed: " + ex.Message);
            return false;
        }
    }

    protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
    {
        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized");
    }
}
