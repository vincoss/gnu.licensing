using Shot.Licensing.Api.ViewModels;
using System;
using System.Threading.Tasks;


namespace Shot.Licensing.Api.Interface
{
    public interface ILicenseRegistrationService
    {
        Task<Guid> Create(LicenseRegistrationViewModel model, string createdByUser);
    }
}
