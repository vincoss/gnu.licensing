using Gnu.Licensing.Api.Models;
using Gnu.Licensing.Validation;
using System.Threading.Tasks;


namespace Gnu.Licensing.Api.Interface
{
    public interface ILicenseService
    {
        Task<IValidationFailure> ValidateAsync(LicenseRegisterRequest request);
        Task<LicenseRegisterResult> CreateAsync(LicenseRegisterRequest request, string userName);
    }
}
