using samplesl.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml.Linq;


namespace samplesl
{
    public abstract class Basel
    {
        private readonly HttpClient _httpClient;

        public Basel(HttpClient httpClient)
        {
            if(httpClient == null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }
            _httpClient = httpClient; ;
        }

        public Task<LicenseResult> ValidateAsync()
        {
            var task = Task.Run(() =>
            {
                var results = new List<IValidationFailure>();
                License actual = null;

                try
                {
                    using (var stream = LicenseOpenRead())
                    {
                        if (stream == null)
                        {
                            var nf = FailureStrings.Get(FailureStrings.ACT08Code);
                            return new LicenseResult(null, null, new[] { nf });
                        }

                        actual = License.Load(stream);
                    }

                    var failures = ValidateInternal(actual);

                    foreach (var f in failures)
                    {
                        results.Add(f);
                    }

                    return new LicenseResult(results.Any() ? null : actual, null, results);
                }
                catch (Exception ex)
                {
                    // TODO: log

                    var failure = FailureStrings.Get(FailureStrings.ACT09Code);

                    results.Add(failure);
                    return new LicenseResult(null, ex, results);
                }
            });
            return task;
        }

        public async Task<LicenseResult> RegisterAsync(Guid licenseKey, Guid productId, string url, IDictionary<string, string> attributes)
        {
            if (licenseKey == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(licenseKey));
            }
            if (productId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(productId));
            }
            if(string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }
            if(attributes == null)
            {
                attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }

            try
            {
                var result = await Register(licenseKey, productId, attributes, url);

                if (string.IsNullOrWhiteSpace(result.License))
                {
                    return new LicenseResult(null, null, new[] { result.Failure });
                }

                using (var sw = LicenseOpenWrite())
                {
                    var element = XElement.Parse(result.License);
                    element.Save(sw);
                }

                var validationResult = await ValidateAsync();
                if (validationResult.Failures.Any())
                {
                    return new LicenseResult(null, null, validationResult.Failures);
                }

                return new LicenseResult(validationResult.License, null, null);
            }
            catch (Exception ex)
            {
                var failure = FailureStrings.Get(FailureStrings.ACT02Code);
                return new LicenseResult(null, ex, new[] { failure });
                // TODO: log
            }
        }

        protected abstract IEnumerable<IValidationFailure> ValidateInternal(License actual);

        protected abstract Stream LicenseOpenRead();

        protected abstract Stream LicenseOpenWrite();

        #region Http

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
            if (string.IsNullOrWhiteSpace(uri))
            {
                throw new ArgumentNullException(nameof(uri));
            }
            if (string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentNullException(nameof(data));
            }

            var content = new StringContent(data);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(uri, content);

            await HandleResponse(response);
            string serialized = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            TResult result = await Task.Run(() => JsonSerializer.Deserialize<TResult>(serialized, options));

            return result;
        }

        public static HttpClient CreateHttpClient()
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

        #endregion
    }
}
