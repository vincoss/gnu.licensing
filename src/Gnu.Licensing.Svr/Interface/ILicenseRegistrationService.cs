using Gnu.Licensing.Svr.ViewModels;
using System;
using System.Threading.Tasks;


namespace Gnu.Licensing.Svr.Interface
{
    public interface ILicenseRegistrationService
    {
        Task<Guid> Create(LicenseRegistrationViewModel model, string createdByUser);
    }
}
