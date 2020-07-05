using Warthog.Api.ViewModels;
using System;
using System.Threading.Tasks;


namespace Warthog.Api.Interface
{
    public interface ILicenseRegistrationService
    {
        Task<Guid> Create(LicenseRegistrationViewModel model, string createdByUser);
    }
}
