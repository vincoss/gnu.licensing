using RichardSzalay.MockHttp;
using Warthog;
using Warthog.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Linq;
using Xunit;
using System.Threading.Tasks;

namespace Warthog
{
    public class BaseLicenseServiceTest
    {
        [Fact]
        public async void Register_ErrorRegister()
        {
            var mockHandler = new MockHttpMessageHandler(); var dir = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data", "test.license.xml");
            var path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data", "test.license.xml");

            var data = new
            {
                License = "",
                Failure = new GeneralValidationFailure
                {
                    Code = "01",
                    Message = nameof(Register_ErrorRegister),
                    HowToResolve = "Fix the test"
                }
            };

            var json = JsonSerializer.Serialize(data);

            mockHandler
                .When("/api/license")
                .Respond("application/json", json);

            var client = mockHandler.ToHttpClient();

            var service = new TestBaseLicenseService(client);
            service.Path = path;
            var result = await service.RegisterAsync(Guid.NewGuid(), Guid.NewGuid(), "http://test.com/api/license", new Dictionary<string, string>());

            Assert.False(result.Successful);
            Assert.Null(result.License);
            Assert.Null(result.Exception);
            Assert.True(result.Failures.Any());
            Assert.Equal("01", result.Failures.ElementAt(0).Code);
            Assert.Equal(nameof(Register_ErrorRegister), result.Failures.ElementAt(0).Message);
            Assert.Equal("Fix the test", result.Failures.ElementAt(0).HowToResolve);
        }

        [Fact]
        public async void Register_Success()
        {
            var mockHandler = new MockHttpMessageHandler(); var dir = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data", "test.license.xml");
            var readPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data", "test.license.xml");
            var writePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data", $"{nameof(Register_Success)}.license.xml");

            var data = new
            {
                License = File.ReadAllText(readPath)
            };

            var json = JsonSerializer.Serialize(data);

            mockHandler
                .When("/api/license")
                .Respond("application/json", json);

            var client = mockHandler.ToHttpClient();

            var service = new TestBaseLicenseService(client);
            service.Path = writePath;
            var result = await service.RegisterAsync(Guid.NewGuid(), Guid.NewGuid(), "http://test.com/api/license", new Dictionary<string, string>());

            Assert.True(result.Successful);
            Assert.NotNull(result.License);
            Assert.Null(result.Exception);
            Assert.False(result.Failures.Any());
        }

        [Fact]
        public async void Register_XmlParseTest()
        {
            var mockHandler = new MockHttpMessageHandler(); var dir = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data", "test.license.xml");
            var readPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data", "test.license.xml");
            var writePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data", $"{nameof(Register_XmlParseTest)}.license.xml");

            var data = new
            {
                License = "xml",
            };

            var json = JsonSerializer.Serialize(data);

            mockHandler
                .When("/api/license")
                .Respond("application/json", json);

            var client = mockHandler.ToHttpClient();

            var service = new TestBaseLicenseService(client);
            service.Path = writePath;
            var result = await service.RegisterAsync(Guid.NewGuid(), Guid.NewGuid(), "http://test.com/api/license", new Dictionary<string, string>());

            Assert.False(result.Successful);
            Assert.Null(result.License);
            Assert.NotNull(result.Exception);
            Assert.True(result.Failures.Any());
            Assert.Equal("ACT.02", result.Failures.ElementAt(0).Code);
        }

        [Fact]
        public async void Register_ValidationFailure()
        {
            var mockHandler = new MockHttpMessageHandler();
            var readPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data", "test.license.xml");
            var writePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data", $"{nameof(Register_ValidationFailure)}.license.xml");

            var data = new
            {
                License = File.ReadAllText(readPath)
            };

            var json = JsonSerializer.Serialize(data);

            mockHandler
                .When("/api/license")
                .Respond("application/json", json);

            var client = mockHandler.ToHttpClient();

            var service = new TestBaseLicenseService(client);
            service.Path = writePath;
            service.Failures = new[]
            {
                new GeneralValidationFailure
                {
                    Code = "test",
                    Message = "message",
                    HowToResolve = "howtoresolve"
                }
            };

            var result = await service.RegisterAsync(Guid.NewGuid(), Guid.NewGuid(), "http://test.com/api/license", new Dictionary<string, string>());

            Assert.False(result.Successful);
            Assert.Null(result.License);
            Assert.Null(result.Exception);
            Assert.True(result.Failures.Any());
            Assert.Equal("test", result.Failures.ElementAt(0).Code);
        }

        [Fact]
        public async void ValidateAsync_NoLicense()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data", "none.license.xml");

            var mockHandler = new MockHttpMessageHandler();
            var client = mockHandler.ToHttpClient();

            var service = new TestBaseLicenseService(client);
            service.Path = path;

            var result = await service.ValidateAsync();

            Assert.False(result.Successful);
            Assert.Null(result.License);
            Assert.Null(result.Exception);
            Assert.Equal("VAL.00", result.Failures.ElementAt(0).Code);
        }

        [Fact]
        public async void ValidateAsync_Exception()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data", "test.license.xml");

            var mockHandler = new MockHttpMessageHandler();
            var client = mockHandler.ToHttpClient();
            var service = new TestBaseLicenseService(client);
            service.Path = path;
            service.Exception = new Exception("Test");

            var result = await service.ValidateAsync();

            Assert.False(result.Successful);
            Assert.Null(result.License);
            Assert.NotNull(result.Exception);
            Assert.Equal("VAL.01", result.Failures.ElementAt(0).Code);
        }

        [Fact]
        public async void ValidateAsync_Invalid()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data", "test.license.xml");

            var mockHandler = new MockHttpMessageHandler();
            var client = mockHandler.ToHttpClient();
            var service = new TestBaseLicenseService(client);
            service.Path = path;
            service.Failures = new[]
            {
                new GeneralValidationFailure
                {
                    Code = "test",
                    Message = "message",
                    HowToResolve = "howtoresolve"
                }
            };

            var result = await service.ValidateAsync();

            Assert.False(result.Successful);
            Assert.Null(result.License);
            Assert.Null(result.Exception);
            Assert.Equal("test", result.Failures.ElementAt(0).Code);
        }

        [Fact]
        public async void ValidateAsync_Valid()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data", "test.license.xml");

            var mockHandler = new MockHttpMessageHandler();
            var client = mockHandler.ToHttpClient();
            var service = new TestBaseLicenseService(client);
            service.Path = path;

            var result = await service.ValidateAsync();

            Assert.True(result.Successful);
            Assert.NotNull(result.License);
            Assert.Null(result.Exception);
            Assert.False(result.Failures.Any());
        }

        private class TestBaseLicenseService : BaseLicenseService
        {
            public string Path;
            public IEnumerable<IValidationFailure> Failures = new IValidationFailure[0];
            public Exception Exception = null;


            public TestBaseLicenseService(HttpClient client) : base(client)
            { }

            protected override Stream LicenseOpenRead()
            {
                if(File.Exists(Path) == false)
                {
                    return null;
                }
                return File.OpenRead(Path);
            }

            protected override Stream LicenseOpenWrite()
            {
                return File.Open(Path, FileMode.Create);
            }

            protected override Task<IEnumerable<IValidationFailure>> ValidateInternal(License actual)
            {
                if(Exception != null)
                {
                    throw Exception;
                }
                return Task.FromResult(Failures);
            }
        }

    }
}
