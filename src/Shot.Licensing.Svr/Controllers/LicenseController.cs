using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shot.Licensing.Svr.Interface;
using Shot.Licensing.Svr.Models;
using Shot.Licensing.Svr.Services;

namespace Shot.Licensing.Svr.Controllers
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
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        //// GET: api/License/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/License
        [HttpPost]
        public async Task<ActionResult<string>> Post([FromBody] RegisterLicense register)
        {
            if (register == null)
            {
                return BadRequest();
            }
            return await _licenseService.Create(register);
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
}
