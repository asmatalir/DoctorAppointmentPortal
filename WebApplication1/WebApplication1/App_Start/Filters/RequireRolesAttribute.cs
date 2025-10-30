// App_Start/Filters/RequireRolesAttribute.cs
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;

public class RequireRolesAttribute : AuthorizeAttribute
{
    protected override bool IsAuthorized(HttpActionContext ctx)
    {
        var user = ctx.RequestContext.Principal as ClaimsPrincipal;
        if (user?.Identity?.IsAuthenticated != true) return false;
        if (string.IsNullOrWhiteSpace(Roles)) return true;

        var need = Roles.Split(',').Select(r => r.Trim());
        var have = user.FindAll(ClaimTypes.Role).Select(c => c.Value);
        return need.Intersect(have, System.StringComparer.OrdinalIgnoreCase).Any();
    }
}
