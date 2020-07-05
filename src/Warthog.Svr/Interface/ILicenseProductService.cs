using Warthog.Api.ViewModels;
using System;
using System.Threading.Tasks;


namespace Warthog.Api.Interface
{
    public interface ILicenseProductService
    {
        Task<Guid> Create(LicenseProductViewModel model, string createdByUser);
    }
}
