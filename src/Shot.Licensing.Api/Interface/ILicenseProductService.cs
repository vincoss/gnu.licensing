using Shot.Licensing.Api.ViewModels;
using System;
using System.Threading.Tasks;


namespace Shot.Licensing.Api.Interface
{
    public interface ILicenseProductService
    {
        Task<Guid> Create(LicenseProductViewModel model, string createdByUser);
    }
}
