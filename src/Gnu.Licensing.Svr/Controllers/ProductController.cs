using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gnu.Licensing.Svr.ViewModels;
using Gnu.Licensing.Svr.Interface;
using Gnu.Licensing.Svr.Models;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Authorization;

namespace Gnu.Licensing.Svr.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/product")]
    public class ProductController : Controller
    {
        private readonly ILicenseProductService _productService;

        public ProductController(ILicenseProductService productService)
        {
            if (productService == null) throw new ArgumentNullException(nameof(productService));

            _productService = productService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LicenseProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            await _productService.Create(model, "todo");

            return Ok();
        }
    }
}
