using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using samplesl.Validation;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Essentials;
using System.Security.Cryptography;

namespace samplesl.Sample_XamarinForms.Services
{
    public class LicenseService : Basel, ILicenseService
    {
        public LicenseService(HttpClient client) : base(client)
        {
        }

        protected override IEnumerable<IValidationFailure> ValidateInternal(License actual)
        {
            var failure = FailureStrings.Get(FailureStrings.ACT14Code);

            var appId = GetAttributes()[LicenseContants.AppId];

            var failures = actual.Validate()
                                        .ExpirationDate()
                                        .When(lic => lic.Type == LicenseType.Standard)
                                        .And()
                                        .Signature(LicenseContants.PublicKey)
                                        .And()
                                        .AssertThat(x => string.Equals(appId, x.AdditionalAttributes.Get(LicenseContants.AppId), StringComparison.OrdinalIgnoreCase), failure)
                                        .AssertValidLicense().ToList();

            return failures;
        }

        protected override Stream LicenseOpenRead()
        {
            if(File.Exists(GetPath()))
            {
                return File.OpenRead(GetPath());
            }
            return null;
        }

        protected override Stream LicenseOpenWrite()
        {
            return File.Open(GetPath(), FileMode.Create);
        }

        public string GetPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "license.xml");
        }

        protected virtual IDictionary<string, string> GetAttributes()
        {
            var id = Preferences.Get(LicenseContants.AppId, null);
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new InvalidOperationException("Missing app ID");
            }

            var attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            attributes.Add(LicenseContants.AppId, id);
            return attributes;
        }

        public Task SetLicenseKeyAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task Run()
        {
            throw new NotImplementedException();
        }
























        //private const string LicenseKey = "LicenseKey";
        //public static LicenseType LicenseType { get; private set; }


        //public Task<string> GetLicenseKeyAsync()
        //{
        //    return SecureStorage.GetAsync(LicenseKey);
        //}

        //public Task SetLicenseKeyAsync(string key)
        //{
        //    return SecureStorage.SetAsync(LicenseKey, key);
        //}

        //public Guid GetProductKey()
        //{
        //    return LicenseContants.ProductId;
        //}

        //public async Task<LicenseResult> RegisterAsync(Guid licenseKey, Guid productId)
        //{
        //    if (licenseKey == Guid.Empty)
        //    {
        //        throw new ArgumentNullException(nameof(licenseKey));
        //    }
        //    if (productId == Guid.Empty)
        //    {
        //        throw new ArgumentNullException(nameof(productId));
        //    }

        //    try
        //    {
        //        var attributes = GetAttributes();
        //        var result = await Register(licenseKey, productId, attributes, LicenseContants.LicenseServerUrl);

        //        if (string.IsNullOrWhiteSpace(result.License))
        //        {
        //            return new LicenseResult(null, null, new[] { result.Failure });
        //        }

        //        var path = GetPath();
        //        var element = XElement.Parse(result.License);
        //        element.Save(path);

        //        var validationResult = await Validate();
        //        if (validationResult.Failures.Any())
        //        {
        //            return new LicenseResult(null, null, validationResult.Failures);
        //        }

        //        return new LicenseResult(null, null, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new LicenseResult(null, ex, null);
        //        // TODO: log
        //    }
        //}

        //public Task<LicenseResult> Validate()
        //{
        //    var task = Task.Run(() =>
        //    {
        //        var results = new List<IValidationFailure>();
        //        License actual = null;

        //        try
        //        {
        //            var path = GetPath();

        //            if (File.Exists(path) == false)
        //            {
        //                var nf = FailureStrings.Get(FailureStrings.ACT08Code);

        //                return new LicenseResult(null, null, new[] { nf });
        //            }

        //            using (var stream = File.OpenRead(path))
        //            {
        //                actual = License.Load(stream);
        //            }

        //            var failure = FailureStrings.Get(FailureStrings.ACT14Code);

        //            var appId = GetAttributes()[LicenseContants.AppId];

        //            var failures = actual.Validate()
        //                                 .ExpirationDate()
        //                                 .When(lic => lic.Type == LicenseType.Standard)
        //                                 .And()
        //                                 .Signature(LicenseContants.PublicKey)
        //                                 .And()
        //                                 .AssertThat(x => string.Equals(appId, x.AdditionalAttributes.Get(LicenseContants.AppId), StringComparison.OrdinalIgnoreCase), failure)
        //                                 .AssertValidLicense().ToList();

        //            foreach (var f in failures)
        //            {
        //                results.Add(f);
        //            }

        //            return new LicenseResult(results.Any() ? null : actual, null, results);
        //        }
        //        catch (Exception ex)
        //        {
        //            // TODO: log

        //            var failure = FailureStrings.Get(FailureStrings.ACT09Code);

        //            results.Add(failure);
        //            return new LicenseResult(null, ex, results);
        //        }
        //    });
        //    return task;
        //}

        //public async Task Run()
        //{
        //    var valid = false;

        //    try
        //    {
        //        var path = GetPath();

        //        // Get license ID from file if exists.
        //        var licenseKey = await TryGetLicenseKeyAsync(path);

        //        if (licenseKey == Guid.Empty)
        //        {
        //            // Get license ID from device store.
        //            var value = await GetLicenseKeyAsync();

        //            if (string.IsNullOrWhiteSpace(value) == false)
        //            {
        //                Guid.TryParse(value, out licenseKey);
        //            }
        //        }

        //        // No license ID or license file. Get license ID from purchase email. If no email use license recovery on the website to get license ID.
        //        if (licenseKey == Guid.Empty)
        //        {
        //            return;
        //        }

        //        var validationResult = await Validate();
        //        valid = validationResult.Successful;

        //        // Let see whether is also valid on license server if has internet connection.
        //        if (await HasConnection(LicenseContants.LicenseServerUrl))
        //        {
        //            var licenseHash = GetHashSHA256File(path);
        //            var productKey = GetProductKey();
        //            var check = await Check(licenseKey, productKey, licenseHash, LicenseContants.LicenseServerUrl);

        //            if (check != null)
        //            {
        //                await RegisterAsync(licenseKey, productKey);
        //            }

        //            validationResult = await Validate();
        //            valid = validationResult.Successful;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex); // TODO:
        //    }
        //    finally
        //    {
        //        if (valid)
        //        {
        //            LicenseType = LicenseType.Standard;
        //        }
        //        else
        //        {
        //            // Not valid, possible new device ID or installation. Use license UI to get license for the device|installation.
        //            LicenseType = LicenseType.Trial;
        //        }
        //    }
        //}

        //public async Task<Guid> TryGetLicenseKeyAsync(string fileName)
        //{
        //    if (string.IsNullOrWhiteSpace(fileName))
        //    {
        //        throw new ArgumentNullException(nameof(fileName));
        //    }

        //    if (File.Exists(fileName) == false)
        //    {
        //        return Guid.Empty;
        //    }

        //    var id = Guid.Empty;
        //    var lic = await Get(fileName);
        //    if (lic != null)
        //    {
        //        id = lic.Id;
        //    }
        //    return id;
        //}

        //public Task<License> Get(string fileName)
        //{
        //    if (string.IsNullOrWhiteSpace(fileName))
        //    {
        //        throw new ArgumentNullException(nameof(fileName));
        //    }
        //    License license = null;
        //    try
        //    {
        //        license = License.Load(File.OpenRead(fileName));
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex); // TODO: logger
        //    }
        //    return Task.FromResult(license);
        //}


        //public string GetHashSHA256File(string fileName)
        //{
        //    if (string.IsNullOrWhiteSpace(fileName))
        //    {
        //        throw new ArgumentNullException(nameof(fileName));
        //    }
        //    using (var sha256 = new SHA256Managed())
        //    using (var stream = new BufferedStream(File.OpenRead(fileName), 100000))
        //    {
        //        return BitConverter.ToString(sha256.ComputeHash(stream)).Replace("-", string.Empty);
        //    }
        //}

        //#region Internal

        //public async Task<bool> HasConnection(string serverUrl)
        //{
        //    if (string.IsNullOrWhiteSpace(serverUrl))
        //    {
        //        throw new ArgumentNullException(nameof(serverUrl));
        //    }
        //    try
        //    {
        //        var client = CreateHttpClient();
        //        using (var response = await client.GetAsync(serverUrl, HttpCompletionOption.ResponseHeadersRead))
        //        {
        //            response.EnsureSuccessStatusCode();
        //            return true;
        //        }
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public async Task<IValidationFailure> Check(Guid licenseKey, Guid productId, string licenseSha256, string serverUrl)
        //{
        //    if (licenseKey == Guid.Empty)
        //    {
        //        throw new ArgumentNullException(nameof(licenseKey));
        //    }
        //    if (productId == Guid.Empty)
        //    {
        //        throw new ArgumentNullException(nameof(productId));
        //    }
        //    if (string.IsNullOrWhiteSpace(licenseSha256))
        //    {
        //        throw new ArgumentNullException(nameof(licenseSha256));
        //    }
        //    if (string.IsNullOrWhiteSpace(serverUrl))
        //    {
        //        throw new ArgumentNullException(nameof(serverUrl));
        //    }

        //    var data = new
        //    {
        //        LicenseId = licenseKey,
        //        ProductId = productId,
        //        LicenseSha256 = licenseSha256
        //    };

        //    var json = JsonSerializer.Serialize(data);
        //    var result = await PostAsync<IValidationFailure>(serverUrl, json);
        //    return result;
        //}

        //#endregion
    }
}
