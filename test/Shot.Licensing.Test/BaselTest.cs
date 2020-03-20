using RichardSzalay.MockHttp;
using samplesl;
using samplesl.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Linq;
using Xunit;

namespace samplesl
{
    public class BaselTest
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

            var service = new TestBasel(client);
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
            var path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data", "test.license.xml");

            var data = new
            {
                License = File.ReadAllText(path)
            };

            var json =  JsonSerializer.Serialize(data);

            mockHandler
                .When("/api/license")
                .Respond("application/json", json);

            var client = mockHandler.ToHttpClient();

            var service = new TestBasel(client);
            service.Path = path;
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
            var path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Data", "test.license.xml");

            var data = new
            {
                License = "xml",
            };

            var json = JsonSerializer.Serialize(data);

            mockHandler
                .When("/api/license")
                .Respond("application/json", json);

            var client = mockHandler.ToHttpClient();

            var service = new TestBasel(client);
            service.Path = path;
            var result = await service.RegisterAsync(Guid.NewGuid(), Guid.NewGuid(), "http://test.com/api/license", new Dictionary<string, string>());

            Assert.False(result.Successful);
            Assert.Null(result.License);
            Assert.NotNull(result.Exception);
            Assert.True(result.Failures.Any());
            Assert.Equal("ACT.02", result.Failures.ElementAt(0).Code);
        }

        class TestBasel : Basel
        {
            public string Path;

            public TestBasel(HttpClient client) : base(client)
            { }

            protected override Stream LicenseOpenRead()
            {
                return File.OpenRead(Path);
            }

            protected override Stream LicenseOpenWrite()
            {
                return File.Open(Path, FileMode.Create);
            }

            protected override IEnumerable<IValidationFailure> ValidateInternal(License actual)
            {
                return new IValidationFailure[0];
            }
        }

    }
}
