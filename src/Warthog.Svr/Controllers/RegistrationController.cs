using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Warthog.Api.Interface;
using Warthog.Api.Models;
using Warthog.Api.ViewModels;

namespace Warthog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly ILicenseRegistrationService _registrationService;

        public RegistrationController(ILicenseRegistrationService registrationService)
        {
            if (registrationService == null) throw new ArgumentNullException(nameof(registrationService));

            _registrationService = registrationService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LicenseRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            await _registrationService.Create(model, "todo");

            return Ok();
        }
    }
}
