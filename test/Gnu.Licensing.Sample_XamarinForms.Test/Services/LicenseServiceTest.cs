using RichardSzalay.MockHttp;
using Gnu.Licensing.Sample_XamarinForms.Services;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using Gnu.Licensing.Validation;
using NSubstitute;
using Gnu.Licensing.Sample_XF;

namespace Gnu.Licensing.Sample_XamarinForms.Test.Services
{
    public class LicenseServiceTest
    {
        [Fact]
        public async void ValidateAsync_NoLicense()
        {
            var mockHandler = new MockHttpMessageHandler();
            var client = mockHandler.ToHttpClient();

            var actx = Substitute.For<IApplicationContext>();
            var appId = Guid.NewGuid().ToString();
            actx.GetAppId().Returns(appId);

            var service = new LicenseService(actx, client);

            var result = await service.ValidateAsync();

            Assert.False(result.Successful);
            Assert.Null(result.License);
            Assert.Null(result.Exception);
            Assert.Equal("VAL.00", result.Failures.ElementAt(0).Code);
        }

        [Fact]
        public async void ValidateAsync_Exception()
        {
            var mockHandler = new MockHttpMessageHandler();
            var client = mockHandler.ToHttpClient();
            
            var actx = Substitute.For<IApplicationContext>();
            var appId = Guid.NewGuid().ToString();
            actx.GetAppId().Returns(appId);

            var service = new LicenseService(actx, client);

            var path = service.GetPath();

            try
            {
                File.WriteAllText(path, "");

                var result = await service.ValidateAsync();

                Assert.False(result.Successful);
                Assert.Null(result.License);
                Assert.NotNull(result.Exception);
                Assert.Equal("VAL.01", result.Failures.ElementAt(0).Code);
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
            
            var actx = Substitute.For<IApplicationContext>();
            var appId = Guid.NewGuid().ToString();
            actx.GetAppId().Returns(appId);

            var service = new TestLicenseService(actx, client);
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
                Assert.Equal("VAL.04", result.Failures.ElementAt(0).Code);
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

            var actx = Substitute.For<IApplicationContext>();
            var appId = new Guid("ae8cdf5f-26b3-4e2f-8e68-6ecc2e73720f");
            actx.GetAppId().Returns(appId.ToString());

            var service = new TestLicenseService(actx, client);
            var path = service.GetPath();
            service.AppId = appId.ToString();

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
            
            var actx = Substitute.For<IApplicationContext>();
            var appId = Guid.NewGuid().ToString();
            actx.GetAppId().Returns(appId);

            var service = new LicenseService(actx, client);

            var result = await service.Register(Guid.NewGuid(), Guid.NewGuid(), new Dictionary<string, string>(), LicenseGlobals.LicenseServerUrl);

            Assert.Equal("200", result.License);
            Assert.Equal("code", result.Failure.Code);

        }

        [Fact]
        public async void Run_Test()
        {
            var mockHandler = new MockHttpMessageHandler();
            var client = mockHandler.ToHttpClient();

            var actx = Substitute.For<IApplicationContext>();
            var appId = Guid.NewGuid().ToString();
            actx.GetAppId().Returns(appId);

            var service = new TestLicenseService(actx, client);
            var path = service.GetPath();
            service.AppId = "B3BCC7E4-6FC8-4CD7-A640-F50B1E5FC95B";

            var dir = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data", "test.license.xml");

            try
            {
                File.WriteAllText(path, File.ReadAllText(dir));

                await service.RunAsync();

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

            public TestLicenseService(IApplicationContext actx, HttpClient client) : base(actx, client)
            { }

            protected override IDictionary<string, string> GetAttributes()
            {
                var id = AppId; // Fake // Preferences.Get(LicenseContants.AppId, null);
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new InvalidOperationException("Missing app ID");
                }

                var attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                attributes.Add(LicenseGlobals.AppId, id);
                return attributes;
            }
        }

    }
}
