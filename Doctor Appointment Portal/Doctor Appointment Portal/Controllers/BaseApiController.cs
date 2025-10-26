using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace DoctorAppointmentPortal.Controllers
{
    public class BaseApiController : ApiController
    {
        protected int CurrentUserId
        {
            get
            {
                var identity = User?.Identity as ClaimsIdentity;
                var userIdClaim = identity?.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    int id;
                    if (int.TryParse(userIdClaim.Value, out id))
                        return id;
                }


                return 0;
            }
        }

        protected string CurrentUsername
        {
            get
            {
                var identity = User?.Identity as ClaimsIdentity;
                return identity?.FindFirst(ClaimTypes.Name)?.Value;
            }
        }

        protected string CurrentEmail
        {
            get
            {
                var identity = User?.Identity as ClaimsIdentity;
                return identity?.FindFirst(ClaimTypes.Email)?.Value;
            }
        }

        protected string CurrentRole
        {
            get
            {
                var identity = User?.Identity as ClaimsIdentity;
                return identity?.FindFirst(ClaimTypes.Role)?.Value;
            }
        }

        protected int CurrentDoctorId
        {
            get
            {
                var identity = User?.Identity as ClaimsIdentity;

                // Case-insensitive check
                var doctorIdClaim = identity?.Claims
                    ?.FirstOrDefault(c => c.Type.Equals("DoctorId", StringComparison.OrdinalIgnoreCase));
                int id;
                if (doctorIdClaim != null && int.TryParse(doctorIdClaim.Value, out id))
                    return id;

                return 0;
            }
        }




    }
}
