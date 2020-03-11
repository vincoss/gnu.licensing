using samplesl.Sample_XamarinForms.Services;
using samplesl.Sample_XamarinForms.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using NSubstitute;
using System.IO;
using samplesl.Validation;

namespace samplesl.Sample_XamarinForms.Test.ViewModels
{
    public class LicenseViewModelTest
    {
        [Fact]
        public void InitializeTest_Valid()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "data", "test.license.xml");

            var service = Substitute.For<ILicenseService>();
            var ctx = Substitute.For<IApplicationContext>();
            var model = new LicenseViewModel(service, ctx);

            var appId = Guid.NewGuid().ToString();
            var license = License.Load(File.OpenRead(path));
            var validationResult = new LicenseResult(license, null, null);

            service.Validate().Returns(validationResult);
            ctx.GetValueOrDefault(LicenseContants.AppId, null).Returns(appId);

            model.Initialize();

            Assert.Null(model.RegisterKey);
            Assert.Null(model.ErrorMessage);
            Assert.False(model.ShowError);
            Assert.Equal(appId, model.AppId);
            Assert.Equal(license.Id.ToString(), model.LicenseKey);
            Assert.Equal("Full", model.LicenseType);
            Assert.False(model.IsBusy);
        }

        [Fact]
        public void InitializeTest_Invalid()
        {
            var service = Substitute.For<ILicenseService>();
            var ctx = Substitute.For<IApplicationContext>();
            var model = new LicenseViewModel(service, ctx);

            var appId = Guid.NewGuid().ToString();
            var exception = new Exception("Exception");
            var validation = new[]
            {
                new GeneralValidationFailure
                {
                    Message = "Message",
                    HowToResolve = "HowToResolve"
                }
            };
            var validationResult = new LicenseResult(null, exception, validation);

            service.Validate().Returns(validationResult);
            ctx.GetValueOrDefault(LicenseContants.AppId, null).Returns(appId);

            model.Initialize();

            Assert.Null(model.RegisterKey);
            Assert.Equal("Message|HowToResolve|Exception|", model.ErrorMessage.Replace("\r\n", "|"));
            Assert.True(model.ShowError);
            Assert.Equal(appId, model.AppId);
            Assert.Equal("Demo", model.LicenseType);
            Assert.False(model.IsBusy);
        }
    }
}
