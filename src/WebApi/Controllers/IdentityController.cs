using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        [Route("claims")]
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(
                from c in User.Claims
                select new {c.Type, c.Value,}
            );
        }
    }
}