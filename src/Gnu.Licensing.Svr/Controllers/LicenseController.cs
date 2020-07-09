using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gnu.Licensing.Svr.Interface;
using Gnu.Licensing.Svr.Models;


namespace Gnu.Licensing.Svr.Controllers
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

        /// <summary>
        /// Check if api is available.
        /// </summary>
        [HttpGet]
        public void Get()
        {
            // Used to check if server is available
        }

        /// <summary>
        /// Register license.
        /// </summary>
        /// <param name="request">License requet.</param>
        /// <returns>License.</returns>
        /// <response code="201">Returns the created license.</response>
        /// <response code="400">If the request is null.</response>      
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LicenseRegisterRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            var result = await _licenseService.CreateAsync(request, string.IsNullOrWhiteSpace(User.Identity.Name) ? Environment.UserName: User.Identity.Name);
            return Ok(result);
        }
    }
}
