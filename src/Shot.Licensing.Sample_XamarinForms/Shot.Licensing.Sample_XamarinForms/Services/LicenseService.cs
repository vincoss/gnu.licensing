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
using System.Net.Http.Headers;
using System.Net;


namespace samplesl.Sample_XamarinForms.Services
{
    public class LicenseService : ILicenseService
    {
        public Task<bool> HasConnection()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Check(Guid licenseKey, Guid productId, string licenseSha256, string serverUrl)
        {
            if (licenseKey == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(licenseKey));
            }
            throw new NotImplementedException();
        }

        public async Task<RegisterResult> Register(Guid licenseKey, Guid productId, IDictionary<string, string> attributes, string serverUrl)
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
            var result = await PostAsync<RegisterResult>(serverUrl, json);
            return result;
        }

        public async Task<TResult> PostAsync<TResult>(string uri, string data)
        {
            if(string.IsNullOrWhiteSpace(uri))
            {
                throw new ArgumentNullException(nameof(uri));
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
    }
}
