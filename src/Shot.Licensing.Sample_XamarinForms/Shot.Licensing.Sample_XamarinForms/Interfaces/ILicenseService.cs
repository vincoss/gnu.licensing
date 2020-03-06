using samplesl.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;


namespace samplesl.Sample_XamarinForms.Interfaces
{
    public interface ILicenseService
    {
        Task<bool> HasConnection();

        Task<bool> Check(Guid id, string licenseSha256, string serverUrl);

        Task<string> Register(Guid id, IDictionary<string, string> attributes, string serverUrl);

        Task<IEnumerable<IValidationFailure>> Validate(Stream license, Stream publicKey, string appId);

    }
}
