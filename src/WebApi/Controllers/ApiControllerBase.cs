using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebApi.Model;

namespace WebApi.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        protected ApiControllerBase()
        { }

        protected bool GetAuthenticatedUserId(out string identifier)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == ClaimIdentifiers.UserId)?.Value;
            identifier = sub;

            return true;
        }
    }
}