using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace WebApi.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        protected ApiControllerBase()
        { }

        protected bool GetAuthenticatedUserId(out string identifier)
        {
            const string ns = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
            var sub = User.Claims.FirstOrDefault(c => c.Type == ns)?.Value;
            identifier = sub;

            return true;
        }
    }
}