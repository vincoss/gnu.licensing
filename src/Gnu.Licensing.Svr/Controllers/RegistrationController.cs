using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gnu.Licensing.Api.Interface;
using Gnu.Licensing.Api.Models;
using Gnu.Licensing.Api.ViewModels;

namespace Gnu.Licensing.Api.Controllers
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
