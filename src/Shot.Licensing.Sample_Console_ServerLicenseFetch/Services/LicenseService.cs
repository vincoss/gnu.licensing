using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace samplesl.Sample_Console_ServerLicenseFetch.Services
{
    public class LicenseService : ILicenseService
    {
        public async Task<string> Register(string licenseRequest, IDictionary<string, string> attributes, string serverUrl)
        {
            if (string.IsNullOrWhiteSpace(licenseRequest))
            {
                throw new ArgumentNullException(nameof(licenseRequest));
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
                var data = new { LicenseId = licenseRequest, Attributes = attributes };
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
    }
}
