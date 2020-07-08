using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gnu.Licensing.Api.ViewModels;
using Gnu.Licensing.Api.Interface;
using Gnu.Licensing.Api.Models;
using Microsoft.AspNetCore.Mvc.ViewEngines;


namespace Gnu.Licensing.Api.Controllers
{
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
