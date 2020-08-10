using Gnu.Licensing.Svr.Interface;
using Gnu.Licensing.Svr.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.IO;


namespace Gnu.Licensing.Svr.Controllers
{
    // TODO: Authorize
    [ApiController]
    [Route("api/certificate")]
    public class CertificateController : ControllerBase
    {
        private readonly IOptionsSnapshot<AppSettings> _options;
        private readonly ICertificateService _certificateService;

        public CertificateController(IOptionsSnapshot<AppSettings> options, ICertificateService certificateService)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (certificateService == null) throw new ArgumentNullException(nameof(certificateService));

            _options = options;
            _certificateService = certificateService;
        }

        [HttpPost]
        public IActionResult Install([FromBody] CertificateInstallRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var path = Path.Combine(_options.Value.DataPath, model.Name);
            _certificateService.Install(path, model.Password, model.Thumbprint);

            return Ok();
        }
    }
}
