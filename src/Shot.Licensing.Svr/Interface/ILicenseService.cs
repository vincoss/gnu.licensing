using samplesl.Svr.Models;
using System.Threading.Tasks;


namespace samplesl.Svr.Interface
{
    public interface ILicenseService
    {
        Task<string> Create(RegisterLicense register);
    }
}
