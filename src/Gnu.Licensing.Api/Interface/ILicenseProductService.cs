using Gnu.Licensing.Api.ViewModels;
using System;
using System.Threading.Tasks;


namespace Gnu.Licensing.Api.Interface
{
    public interface ILicenseProductService
    {
        Task<Guid> CreateAsync(LicenseProductViewModel model, string createdByUser);
    }
}
