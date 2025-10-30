using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

public class JwtAuthHandler : DelegatingHandler
{
    private static readonly SymmetricSecurityKey SigningKey =
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["JwtSecret"]));

    private static readonly JwtSecurityTokenHandler TokenHandler = new JwtSecurityTokenHandler();

    private static readonly TokenValidationParameters Tv = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        IssuerSigningKey = SigningKey
    };

    // Map endpoints to allowed roles
    private static readonly Dictionary<string, string[]> RoleMap = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
    {
        { "/api/bookissue/addeditissueBook", new[] { "Admin","Librarian" } },
        { "/api/bookissue/savebookissue", new[] { "Admin" } },
        { "/api/securedata", new[] { "Admin", "Manager", "User" } }
        // Add more mappings as needed
    };

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage req, CancellationToken ct)
    {
        var path = req.RequestUri.AbsolutePath.ToLowerInvariant();

        // Allow anonymous/public paths + CORS preflight
        if (req.Method == HttpMethod.Options
            || path.StartsWith("/api/accounts/login")
            || path.StartsWith("/api/accounts/verifyemail")
            || path.StartsWith("/api/accounts/resetpassword")
            || path.StartsWith("/api/accounts/passwordgenerator"))
        {
            return await base.SendAsync(req, ct);
        }

        var auth = req.Headers.Authorization;
        if (auth == null || !auth.Scheme.Equals("Bearer", StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(auth.Parameter))
            return Unauthorized(req, "Missing Bearer token");

        try
        {
            SecurityToken validatedToken;
            var principal = TokenHandler.ValidateToken(auth.Parameter, Tv, out validatedToken);

            // Set principal for Web API and Thread
            Thread.CurrentPrincipal = principal;
            if (System.Web.HttpContext.Current != null)
                System.Web.HttpContext.Current.User = principal;


            string[] allowedRoles = null;

            // find a key that is a prefix of the path
            var matchingKey = RoleMap.Keys.FirstOrDefault(k => path.StartsWith(k, StringComparison.OrdinalIgnoreCase));

            if (matchingKey != null)
            {
                allowedRoles = RoleMap[matchingKey];

                var userRoles = principal.Claims
                                         .Where(c => c.Type == ClaimTypes.Role)
                                         .Select(c => c.Value);

                if (!userRoles.Any(r => allowedRoles.Contains(r)))
                    return Unauthorized(req, "Forbidden: insufficient role");
            }






            return await base.SendAsync(req, ct);
        }
        catch (SecurityTokenExpiredException)
        {
            return Unauthorized(req, "Token expired");
        }
        catch
        {
            return Unauthorized(req, "Invalid token");
        }
    }

    private static HttpResponseMessage Unauthorized(HttpRequestMessage req, string reason)
    {
        var res = req.CreateResponse(HttpStatusCode.Unauthorized, new { error = reason });
        res.Headers.WwwAuthenticate.ParseAdd(@"Bearer error=""invalid_token""");
        return res;
    }
}
