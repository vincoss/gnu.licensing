using Shot.Licensing.Api.Models;
using Shot.Licensing.Validation;
using System.Threading.Tasks;


namespace Shot.Licensing.Api.Interface
{
    public interface ILicenseService
    {
        Task<IValidationFailure> ValidateAsync(LicenseRegisterRequest request);
        Task<LicenseRegisterResult> CreateAsync(LicenseRegisterRequest request, string userName);
    }
}
