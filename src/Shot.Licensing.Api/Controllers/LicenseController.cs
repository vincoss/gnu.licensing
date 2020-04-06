using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shot.Licensing.Api.Interface;
using Shot.Licensing.Api.Models;


namespace Shot.Licensing.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LicenseController : ControllerBase
    {
        private readonly ILicenseService _licenseService;

        public LicenseController(ILicenseService licenseService)
        {
            if(licenseService ==  null)
            {
                throw new ArgumentNullException(nameof(licenseService));
            }
            _licenseService = licenseService;
        }

        // GET: api/license
        [HttpGet]
        public void Get()
        {
            // Used to check if server is available
        }

        // POST: api/license
        [HttpPost]
        public async Task<ActionResult<LicenseRegisterResult>> Post([FromBody] LicenseRegisterRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            return  await _licenseService.CreateAsync(request, string.IsNullOrWhiteSpace(User.Identity.Name) ? Environment.UserName: User.Identity.Name);
        }
    }
}
