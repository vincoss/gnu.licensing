using Warthog.Api.Models;
using Warthog.Validation;
using System.Threading.Tasks;


namespace Warthog.Api.Interface
{
    public interface ILicenseService
    {
        Task<IValidationFailure> ValidateAsync(LicenseRegisterRequest request);
        Task<LicenseRegisterResult> CreateAsync(LicenseRegisterRequest request, string userName);
    }
}
