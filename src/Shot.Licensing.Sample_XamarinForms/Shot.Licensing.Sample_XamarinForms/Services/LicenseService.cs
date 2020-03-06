using samplesl.Sample_XamarinForms.Interfaces;
using samplesl.Validation;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace samplesl.Sample_XamarinForms.Services
{
    public class LicenseService : ILicenseService
    {
        public Task<bool> HasConnection()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Check(Guid id, string licenseSha256, string serverUrl)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }
            throw new NotImplementedException();
        }

        public async Task<string> Register(Guid id, IDictionary<string, string> attributes, string serverUrl)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (attributes == null)
            {
                throw new ArgumentNullException(nameof(attributes));
            }
            if (string.IsNullOrWhiteSpace(serverUrl))
            {
                throw new ArgumentNullException(nameof(serverUrl));
            }

            string license = null;

            try
            {
                var data = new { LicenseId = id, Attributes = attributes };
                var json = JsonSerializer.Serialize(data);

                using (var client = CreateHttpClient())
                using (var request = new HttpRequestMessage(HttpMethod.Post, serverUrl))
                {
                    using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        request.Content = stringContent;

                        using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                        {
                            response.EnsureSuccessStatusCode();
                            license = await response.Content.ReadAsStringAsync();
                        }
                    }
                }
            }
            catch
            {
                license = null;
            }
            return license;
        }

        private HttpClient CreateHttpClient() // TODO: must reuse client on the server.
        {
            var httpClient = new HttpClient();
            return httpClient;
        }

        public Task<IEnumerable<IValidationFailure>> Validate(Stream license, Stream publicKey, string appId)
        {
            if (license == null)
            {
                throw new ArgumentNullException(nameof(license));
            }
            if (publicKey == null)
            {
                throw new ArgumentNullException(nameof(publicKey));
            }
            if (string.IsNullOrWhiteSpace(appId))
            {
                throw new ArgumentNullException(nameof(appId));
            }

            var task = Task.Run(() =>
            {
                var results = new List<IValidationFailure>();

                try
                {
                    var actual = License.Load(license);

                    var failure = new GeneralValidationFailure()
                    {
                        Message = "The license is not valid for current device.",
                        HowToResolve = "Please use license ID to register current installation or a device."
                    };

                    var validationFailures = actual.Validate()
                                                   .ExpirationDate()
                                                   .When(lic => lic.Type == LicenseType.Standard)
                                                   .And()
                                                   .Signature(LicenseContants.PublicKey)
                                                   .And()
                                                   .AssertThat(x => string.Equals(appId, x.AdditionalAttributes.Get(LicenseContants.AppId), StringComparison.OrdinalIgnoreCase), failure)
                                                   .AssertValidLicense().ToList();

                    foreach (var f in validationFailures)
                    {
                        results.Add(f);
                    }
                }
                catch (Exception ex)
                {
                    // TODO: replace with logger
                    Console.WriteLine(ex);

                    var exceptionFailure = new GeneralValidationFailure()
                    {
                        Message = "Invalid license file.",
                        HowToResolve = "Please use license ID to register current installation or a device."
                    };

                    results.Add(exceptionFailure);
                }
                return results.AsEnumerable();
            });
            return task;
        }
    }
}
