using Shot.Licensing.Svr.Models;
using Shot.Licensing.Validation;
using System.Threading.Tasks;


namespace Shot.Licensing.Svr.Interface
{
    public interface ILicenseService
    {
        Task<IValidationFailure> ValidateAsync(LicenseRegisterRequest request);
        Task<LicenseRegisterResult> CreateAsync(LicenseRegisterRequest request);
    }
}
