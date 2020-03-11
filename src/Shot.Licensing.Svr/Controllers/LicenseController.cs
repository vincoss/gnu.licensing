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

        public LicenseController() : this(new LicenseService())
        {
        }

        protected LicenseController(ILicenseService licenseService)
        {
            if(licenseService ==  null)
            {
                throw new ArgumentNullException(nameof(licenseService));
            }
            _licenseService = licenseService;
        }

        // GET: api/License
        [HttpGet]
        public void Get()
        {
        }

        //// GET: api/License
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/License/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/License
        [HttpPost]
        public async Task<ActionResult<LicenseRegisterResult>> Post([FromBody] RegisterLicense register)
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

        //// PUT: api/License/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }

    public class LicenseRegisterResult
    {
        public string License { get; set; }
        public IValidationFailure Failure { get; set; }

    }
}
