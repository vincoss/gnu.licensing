using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using samplesl.Svr.Interface;
using samplesl.Svr.Models;
using samplesl.Svr.Services;
using samplesl.Validation;


namespace samplesl.Svr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LicenseController : ControllerBase
    {
        private readonly ILicenseService _licenseService;
        private readonly IDataStoreSvr _dataStore;

        public LicenseController(ILicenseService licenseService, IDataStoreSvr dataStore)
        {
            if(licenseService ==  null)
            {
                throw new ArgumentNullException(nameof(licenseService));
            }
            if(dataStore == null)
            {
                throw new ArgumentNullException(nameof(dataStore));
            }
            _licenseService = licenseService;
            _dataStore = dataStore;
        }

        // GET: api/license
        [HttpGet]
        public void Get()
        {
            // Used to check if server is available
        }

        // POST: api/license
        [HttpPost]
        public async Task<ActionResult<LicenseRegisterResult>> Post([FromBody] LicenseRegisterRequest register)
        {
            if (register == null)
            {
                return BadRequest();
            }
            var str = await _licenseService.Create(register);
            var result = new LicenseRegisterResult();
            result.License = str;
            result.Failure = new GeneralValidationFailure
            {
                Message = nameof(GeneralValidationFailure.Message),
                HowToResolve = nameof(GeneralValidationFailure.HowToResolve)
            };

            return result;
        }
    }
}
