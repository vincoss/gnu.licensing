using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net;
using samplesl.Validation;
using System.Xml.Linq;
using samplesl;
using System.Linq;


namespace samplesl.Sample_XamarinForms.Services
{
    public abstract class BaseLicenseService : ILicenseService
    {

        //Task<bool> HasConnection(string serverUrl);

        //Task<IValidationFailure> Check(Guid licenseKey, Guid productId, string licenseSha256, string serverUrl);

        //Task<LicenseRegisterResult> Register(Guid licenseKey, Guid productId, IDictionary<string, string> attributes, string serverUrl);


        public async Task<bool> HasConnection(string serverUrl)
        {
            if (string.IsNullOrWhiteSpace(serverUrl))
            {
                throw new ArgumentNullException(nameof(serverUrl));
            }
            try
            {
                var client = CreateHttpClient();
                using (var response = await client.GetAsync(serverUrl, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<IValidationFailure> Check(Guid licenseKey, Guid productId, string licenseSha256, string serverUrl)
        {
            if (licenseKey == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(licenseKey));
            }
            if (productId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(productId));
            }
            if (string.IsNullOrWhiteSpace(licenseSha256))
            {
                throw new ArgumentNullException(nameof(licenseSha256));
            }
            if (string.IsNullOrWhiteSpace(serverUrl))
            {
                throw new ArgumentNullException(nameof(serverUrl));
            }

            var data = new
            {
                LicenseId = licenseKey,
                ProductId = productId,
                LicenseSha256 = licenseSha256
            };

            var json = JsonSerializer.Serialize(data);
            var result = await PostAsync<IValidationFailure>(serverUrl, json);
            return result;
        }

        public async Task<LicenseRegisterResult> Register(Guid licenseKey, Guid productId, IDictionary<string, string> attributes, string serverUrl)
        {
            if (licenseKey == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(licenseKey));
            }
            if (productId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(productId));
            }
            if (attributes == null)
            {
                throw new ArgumentNullException(nameof(attributes));
            }
            if (string.IsNullOrWhiteSpace(serverUrl))
            {
                throw new ArgumentNullException(nameof(serverUrl));
            }

            var data = new
            {
                LicenseId = licenseKey,
                ProductId = productId,
                Attributes = attributes
            };

            var json = JsonSerializer.Serialize(data);
            var result = await PostAsync<LicenseRegisterResult>(serverUrl, json);
            return result;
        }

        public async Task<TResult> PostAsync<TResult>(string uri, string data)
        {
            if(string.IsNullOrWhiteSpace(uri))
            {
                throw new ArgumentNullException(nameof(uri));
            }
            if(string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentNullException(nameof(data));
            }

            HttpClient httpClient = CreateHttpClient();

            var content = new StringContent(data);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PostAsync(uri, content);

            await HandleResponse(response);
            string serialized = await response.Content.ReadAsStringAsync();

            TResult result = await Task.Run(() => JsonSerializer.Deserialize<TResult>(serialized));

            return result;
        }

        private HttpClient CreateHttpClient() 
        {
            var httpClient = new HttpClient();
            return httpClient;
        }

        private async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException(content);
            }
        }

        public async Task<LicenseResult> RegisterAsync(Guid licenseKey, Guid productId)
        {
            if (licenseKey == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(licenseKey));
            }
            if (productId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(productId));
            }

            try
            {
                var attributes = GetAttributes();
                var result = await Register(licenseKey, productId, attributes, LicenseContants.LicenseServerUrl);

                if (string.IsNullOrWhiteSpace(result.License))
                {
                    return new LicenseResult(null, null, new[] { result.Failure });
                }

                var path = GetPath();
                var element = XElement.Parse(result.License);
                element.Save(path);

                var validationResult = await Validate();
                if (validationResult.Failures.Any())
                {
                    return new LicenseResult(null, null, validationResult.Failures);
                }

                return new LicenseResult(null, null, null);
            }
            catch (Exception ex)
            {
                return new LicenseResult(null, ex, null);
                // TODO: log
            }
        }

        public Task<License> Get()
        {
            License license = null;
            try
            {
                var path = GetPath();
                license = License.Load(File.OpenRead(path));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); // TODO: logger
            }
            return Task.FromResult(license);
        }

        public abstract IDictionary<string, string> GetAttributes();

        public abstract string GetPath();

        public abstract Task<LicenseResult> Validate();

        public Task SetLicenseKeyAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task Run()
        {
            throw new NotImplementedException();
        }
    }


   
}
