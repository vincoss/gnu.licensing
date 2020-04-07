using RichardSzalay.MockHttp;
using Shot.Licensing.Sample_XamarinForms.Services;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using Shot.Licensing.Validation;

namespace Shot.Licensing.Sample_XamarinForms.Test.Services
{
    public class LicenseServiceTest
    {
        [Fact]
        public async void ValidateAsync_NoLicense()
        {
            var mockHandler = new MockHttpMessageHandler();
            var client = mockHandler.ToHttpClient();

            var service = new LicenseService(client);

            var result = await service.ValidateAsync();

            Assert.False(result.Successful);
            Assert.Null(result.License);
            Assert.Null(result.Exception);
            Assert.Equal("ACT.08", result.Failures.ElementAt(0).Code);
        }

        [Fact]
        public async void ValidateAsync_Exception()
        {
            var mockHandler = new MockHttpMessageHandler();
            var client = mockHandler.ToHttpClient();
            var service = new LicenseService(client);
            var path = service.GetPath();

            try
            {
                File.WriteAllText(path, "");

                var result = await service.ValidateAsync();

                Assert.False(result.Successful);
                Assert.Null(result.License);
                Assert.NotNull(result.Exception);
                Assert.Equal("ACT.09", result.Failures.ElementAt(0).Code);
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Fact]
        public async void ValidateAsync_Invalid()
        {
            var mockHandler = new MockHttpMessageHandler();
            var client = mockHandler.ToHttpClient();
            var service = new TestLicenseService(client);
            var path = service.GetPath();
            service.AppId = "2EEB2CE7-0A1E-442D-BF0B-BB074A3DCFB6";

            var dir = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data", "test.license.xml");
           
            try
            {
                File.WriteAllText(path, File.ReadAllText(dir));

                var result = await service.ValidateAsync();

                Assert.False(result.Successful);
                Assert.Null(result.License);
                Assert.Null(result.Exception);
                Assert.Equal("ACT.14", result.Failures.ElementAt(0).Code);
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Fact]
        public async void ValidateAsync_Valid()
        {
            var mockHandler = new MockHttpMessageHandler();
            var client = mockHandler.ToHttpClient();
            var service = new TestLicenseService(client);
            var path = service.GetPath();
            service.AppId = "B3BCC7E4-6FC8-4CD7-A640-F50B1E5FC95B";

            var dir = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data", "test.license.xml");

            try
            {
                File.WriteAllText(path, File.ReadAllText(dir));

                var result = await service.ValidateAsync();

                Assert.True(result.Successful);
                Assert.NotNull(result.License);
                Assert.Null(result.Exception);
                Assert.False(result.Failures.Any());
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Fact]
        public async void Register_200()
        {
            var mockHandler = new MockHttpMessageHandler();

            var response = new LicenseRegisterResult
            {
                License = "200",
                Failure = new GeneralValidationFailure
                {
                    Code = "code",
                    Message = "message",
                    HowToResolve = "how-to-resolve"
                }
            };

            var str = JsonSerializer.Serialize(response);

            mockHandler
                .When("/api/license")
                .Respond("application/json", str);

            var client = mockHandler.ToHttpClient();

            var service = new LicenseService(client);
            var result = await service.Register(Guid.NewGuid(), Guid.NewGuid(), new Dictionary<string, string>(), LicenseGlobals.LicenseServerUrl);

            Assert.Equal("200", result.License);
            Assert.Equal("code", result.Failure.Code);

        }

        [Fact]
        public async void Run_Test()
        {
            var mockHandler = new MockHttpMessageHandler();
            var client = mockHandler.ToHttpClient();
            var service = new TestLicenseService(client);
            var path = service.GetPath();
            service.AppId = "B3BCC7E4-6FC8-4CD7-A640-F50B1E5FC95B";

            var dir = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data", "test.license.xml");

            try
            {
                File.WriteAllText(path, File.ReadAllText(dir));

                await service.Run();

                Assert.Equal(AppLicense.Full, LicenseGlobals.Get());
            }
            finally
            {
                File.Delete(path);
            }
        }


        class TestLicenseService : LicenseService
        {
            public string AppId;

            public TestLicenseService(HttpClient client) : base(client)
            { }

            protected override IDictionary<string, string> GetAttributes()
            {
                var id = AppId; // Fake // Preferences.Get(LicenseContants.AppId, null);
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new InvalidOperationException("Missing app ID");
                }

                var attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                attributes.Add(LicenseGlobals.AppIdKey, id);
                return attributes;
            }
        }

    }
}
