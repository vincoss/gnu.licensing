using Shot.Licensing.Svr.Models;
using System.Threading.Tasks;


namespace Shot.Licensing.Svr.Interface
{
    public interface ILicenseService
    {
        Task<string> Create(RegisterLicense register);
    }
}
