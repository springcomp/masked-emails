using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Interop;

namespace WebApi.Controllers
{

    [Route("[controller]")]
    public class AddressesController : Controller
    {
        private readonly IMaskedEmailService service_;

        public AddressesController(IMaskedEmailService service)
        {
            service_ = service;
        }

        // GET addresses/{email}
        [HttpGet("{email}")]
        public async Task<ActionResult> GetMaskedEmailAddress(string email)
        {
            var address = await service_.GetMaskedEmail(email);
            return Ok(address);
        }

        // PATCH addresses/{email}/Received
        [HttpPatch("{email}/ReceivedCount")]
        public async Task<ActionResult> UpdateMaskedEmailReceiveCount(string email)
        {
            var address = await service_.IncrementReceiveCount(email);
            return Ok(address);
        }
    }
}