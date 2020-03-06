using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using samplesl;
using samplesl.Sample_XamarinForms.Interfaces;
using samplesl.Sample_XamarinForms.Services;
using System.Xml.Linq;
using System.Security.Cryptography;


namespace samplesl.Sample_XamarinForms
{
    public class Sample
    {
        public async void Activate()
        {
            var helper = new Helper();

            var result = await helper.RegisterAsync(Guid.NewGuid());
            if(result == null)
            {
                // Show generic error
                return;
            }
            // Key: Guid, Type: Full
        }
    }

    public class Helper : BaseHelper
    {
        private const string LicenseKey = "LicenseKey";

        public Helper() : base(new LicenseService())
        {
        }

        public override string GetPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "License.xml");
        }

        public override IDictionary<string, string> GetAttributes()
        {
            var attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            attributes.Add(LicenseContants.AppId, "todo");
            return attributes;
        }

        public override Task<string> GetLicenseKeyAsync()
        {
            return SecureStorage.GetAsync(LicenseKey);
        }

        public override Task SetLicenseKeyAsync(string key)
        {
            return SecureStorage.SetAsync(LicenseKey, key);
        }
    }

    public abstract class BaseHelper
    {
        private readonly ILicenseService _service;
        public static LicenseType LicenseType { get; private set; }

        protected BaseHelper(ILicenseService service)
        {
            if(service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }
            _service = service;
        }

        public abstract string GetPath();

        public abstract Task<string> GetLicenseKeyAsync();

        public abstract Task SetLicenseKeyAsync(string key);

        public abstract IDictionary<string, string> GetAttributes();

        public async Task<License> RegisterAsync(Guid licenseKey)
        {
            if (licenseKey == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(licenseKey));
            }

            License license;

            try
            {
                var attributes = GetAttributes();
                var licStr = await _service.Register(licenseKey, attributes, LicenseContants.LicenseServerUrl);

                if (string.IsNullOrWhiteSpace(licStr))
                {
                    Console.WriteLine("License registration could not complete."); // TODO:
                    return null;
                }

                var path = GetPath();
                var element = XElement.Parse(licStr);
                element.Save(path);

                var valid = await ValidateAsync();

                if (valid == false)
                {
                    return null;
                }

                license = TryGetLicense(path);
                await SetLicenseKeyAsync(licenseKey.ToString());
            }
            catch(Exception ex)
            {
                throw; // TODO:
            }

            return license;
        }

        public async Task<bool> ValidateAsync()
        {
            var path = GetPath();
            using (var publicKey = new MemoryStream(Encoding.UTF8.GetBytes(LicenseContants.PublicKey)))
            using (var license = File.OpenRead(path))
            {
                var result = await _service.Validate(license, publicKey, null); // TODO: AppIdKey
                if (result.Any())
                {
                    return false;
                }
            }
            return true;
        }

        public  string GetHashSHA256File(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }
            using (var sha256 = new SHA256Managed())
            using (var stream = new BufferedStream(File.OpenRead(fileName), 100000))
            {
                return BitConverter.ToString(sha256.ComputeHash(stream)).Replace("-", string.Empty);
            }
        }

        public Guid TryGetLicenseKeyAsync(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            if (File.Exists(fileName) == false)
            {
                return Guid.Empty;
            }

            var id = Guid.Empty;
            var lic = TryGetLicense(fileName);
            if(lic != null)
            {
                id = lic.Id;
            }
            return id;
        }

        public License TryGetLicense(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }
            License license = null;
            try
            {
                license = License.Load(File.OpenRead(fileName));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); // TODO: logger
            }
            return license;
        }

        public async Task Run()
        {
            var valid = false;

            try
            {
                var path = GetPath();

                // Get license ID from file if exists.
                var licenseKey = TryGetLicenseKeyAsync(path);

                if (licenseKey == Guid.Empty)
                {
                    // Get license ID from device store.
                    var value = await GetLicenseKeyAsync();

                    if (string.IsNullOrWhiteSpace(value) == false)
                    {
                        Guid.TryParse(value, out licenseKey);
                    }
                }

                // No license ID or license file. Get license ID from purchase email. If no email use license recovery on the website to get license ID.
                if (licenseKey == Guid.Empty)
                {
                    return;
                }

                valid = await ValidateAsync();
                if (valid)
                {
                    LicenseType = LicenseType.Standard;
                }

                // Let see whether is also valid on license server if has internet connection.
                if (await _service.HasConnection())
                {
                    var licenseHash = GetHashSHA256File(path);
                    var check = await _service.Check(licenseKey, licenseHash, LicenseContants.LicenseServerUrl);

                    if (check == false)
                    {
                        await RegisterAsync(licenseKey);
                    }

                    valid = await ValidateAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                if (valid)
                {
                    LicenseType = LicenseType.Standard;
                }
                else
                {
                    // Not valid, possible new device ID or installation. Use license UI to get license for the device|installation.
                    LicenseType = LicenseType.Trial;
                }
            }
        }

    }

}
