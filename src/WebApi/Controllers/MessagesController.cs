using InboxApi.Model.Interop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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
        private readonly IInboxApi inbox_;
        private readonly ILogger<MessagesController> logger_;

        public MessagesController(IProfilesService service, IInboxApi inbox, ILogger<MessagesController> logger)
        {
            service_ = service;
            inbox_ = inbox;

            logger_ = logger;
        }

        // GET messages/my
        // GET messages/my?location={location}
        [HttpGet()]
        [Route("my")]
        public async Task<IActionResult> GetMessages(string location = null)
        {
            if (string.IsNullOrWhiteSpace(location))
                return await GetInboxMessages();
            else
                return await GetInboxMessage(location);
        }

        private async Task<IActionResult> GetInboxMessages()
        {
            if (!GetAuthenticatedUserId(out var identifier))
                return BadRequest();

            var addresses = (await service_
                    .GetMaskedEmails(identifier))
                    .Select(a => a.EmailAddress)
                    .ToArray()
                    ;

            try
            {
                logger_.LogDebug($"Retrieving messages from the following inboxes:");
                logger_.LogDebug(string.Join(", ", addresses));

                var messages = await inbox_.GetMessages(addresses);

                return Ok(messages);
            }
            catch (Refit.ApiException e)
            {
                return StatusCode((int)e.StatusCode);
            }
        }

        private async Task<IActionResult> GetInboxMessage(string location)
        {
            if (!GetAuthenticatedUserId(out var _))
                return BadRequest();

            try
            {
                logger_.LogDebug($"Retrieving message located at '{location}'.");

                var message = await inbox_.GetMessageBody(location);

                return Ok(message);
            }
            catch (Refit.ApiException e)
            {
                return StatusCode((int)e.StatusCode);
            }
        }
    }
}