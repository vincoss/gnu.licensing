using RichardSzalay.MockHttp;
using samplesl.Sample_XamarinForms.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace samplesl.Sample_XamarinForms.Test.Services
{
   public  class LicenseServiceTest
    {
        [Fact]
        public async void Register_200()
        {
            var mockHandler = new MockHttpMessageHandler();

            mockHandler
                .When("/api/license")
                .Respond("application/json", @"{""license"":""200"",""failure"":{""message"":""message"",""howToResolve"":""howToResolve""}}");

            var client = mockHandler.ToHttpClient();

            var service = new LicenseService(client);
            var result = await service.Register(Guid.NewGuid(), Guid.NewGuid(), new Dictionary<string, string>(), LicenseContants.LicenseServerUrl);

            Assert.Equal("200", result.License);
            Assert.Equal("message", result.Failure.Message);
            Assert.Equal("howToResolve", result.Failure.HowToResolve);

        }
    }
}
