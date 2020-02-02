using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Services.Interop;

namespace WebApi.Controllers
{
    [Route("messages")]
    [ApiController]
    [Authorize]
    public class MessagesController : ApiControllerBase
    {
        private readonly IProfilesService service_;
        private readonly ILogger<MessagesController> logger_;

        public MessagesController(IProfilesService service, ILogger<MessagesController> logger)
        {
            service_ = service;
            logger_ = logger;
        }

        // GET messages/my
        [HttpGet()]
        [Route("my")]
        public async Task<IActionResult> GetMessages()
        {
            if (!GetAuthenticatedUserId(out var identifier))
                return BadRequest();

            var addresses = (await service_
                    .GetMaskedEmails(identifier))
                    .Select(a => a.EmailAddress)
                    .ToArray()
                    ;

            return Ok(addresses);
        }

        // GET messages/my/{location}
        [HttpGet()]
        [Route("my/{location}")]
        public async Task<IActionResult> GetMessageBody(string location)
        {
            if (!GetAuthenticatedUserId(out var identifier))
                return BadRequest();

            return Ok();
        }
    }
}