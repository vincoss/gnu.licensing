using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shot.Licensing.Api.Interface;
using Shot.Licensing.Api.Models;
using Shot.Licensing.Api.ViewModels;

namespace Shot.Licensing.Api.Controllers
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
