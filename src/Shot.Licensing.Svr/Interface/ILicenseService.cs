using samplesl.Svr.Models;
using samplesl.Validation;
using System.Threading.Tasks;


namespace samplesl.Svr.Interface
{
    public interface ILicenseService
    {
        Task<IValidationFailure> ValidateAsync(LicenseRegisterRequest request);
        Task<LicenseRegisterResult> CreateAsync(LicenseRegisterRequest request);
    }
}
