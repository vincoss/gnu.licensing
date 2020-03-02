using Shot.Licensing;
using Shot.Licensing.Validation;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace Shot.Licensing.Sample_Console.Services
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

            var data = new { LicenseId = licenseRequest, Attributes = attributes };
            var json = JsonSerializer.Serialize(data);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, serverUrl))
            {
                using (var stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    request.Content = stringContent;

                    using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                    {
                        response.EnsureSuccessStatusCode();
                        var result = await response.Content.ReadAsStringAsync();
                        return result;
                    }
                }
            }
        }

    
    }
}
