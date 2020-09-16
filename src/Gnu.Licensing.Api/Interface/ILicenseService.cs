using Gnu.Licensing.Svr.Models;
using Gnu.Licensing.Validation;
using System.Threading.Tasks;


namespace Gnu.Licensing.Svr.Interface
{
    public interface ILicenseService
    {
        Task<IValidationFailure> ValidateAsync(LicenseRegisterRequest request);
        Task<LicenseRegisterResult> CreateAsync(LicenseRegisterRequest request, string userName);
    }
}
