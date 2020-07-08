using Gnu.Licensing.Sample_XamarinForms.Services;
using Gnu.Licensing.Sample_XamarinForms.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using NSubstitute;
using System.IO;
using Gnu.Licensing.Validation;

namespace Gnu.Licensing.Sample_XamarinForms.Test.ViewModels
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

            service.ValidateAsync().Returns(validationResult);
            ctx.GetValueOrDefault(LicenseGlobals.AppId, null).Returns(appId);

            model.Initialize();

            Assert.Null(model.RegisterKey);
            Assert.Null(model.ErrorMessage);
            Assert.False(model.ShowError);
            Assert.Equal("Product Name - Full", model.Description);
            Assert.Equal(license.Id.ToString(), model.LicenseKey);
            Assert.True(model.ShowActivated);
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

            service.ValidateAsync().Returns(validationResult);
            ctx.GetValueOrDefault(LicenseGlobals.AppId, null).Returns(appId);

            model.Initialize();

            Assert.Null(model.RegisterKey);
            Assert.Equal("|Message|HowToResolve|Exception|", model.ErrorMessage.Replace("\r\n", "|"));
            Assert.True(model.ShowError);
            Assert.Equal("Product Name - Demo", model.Description);
            Assert.Null(model.LicenseKey);
            Assert.False(model.ShowActivated);
            Assert.False(model.IsBusy);
        }

        [Fact]
        public void OnCanActivateCommand()
        {
            var service = Substitute.For<ILicenseService>();
            var ctx = Substitute.For<IApplicationContext>();
            var model = new LicenseViewModel(service, ctx);

            model.RegisterKey = Guid.NewGuid().ToString();

            var result = model.ActivateCommand.CanExecute(null);

            Assert.True(result);
        }

        [Fact]
        public void OnActivateCommand_Ok()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "data", "test.license.xml");

            var service = Substitute.For<ILicenseService>();
            var ctx = Substitute.For<IApplicationContext>();
            var model = new LicenseViewModel(service, ctx);

            var license = License.Load(File.OpenRead(path));
            var licenseResult = new LicenseResult(license, null, null);

            service.RegisterAsync(Arg.Any<Guid>()).Returns(licenseResult);
            service.ValidateAsync().Returns(licenseResult);

            var key = new Guid("ae8cdf5f-26b3-4e2f-8e68-6ecc2e73720f");
            model.RegisterKey = key.ToString();

            model.ActivateCommand.Execute(null);

            service.Received().RegisterAsync(key);
            service.Received().ValidateAsync();
            ctx.Received().SetLicenseKeyAsync(key.ToString());
            Assert.Equal(AppLicense.Full, LicenseGlobals.Get());
            Assert.False(model.IsBusy);
        }

        [Fact]
        public void OnActivateCommand_Error()
        {
            var service = Substitute.For<ILicenseService>();
            var ctx = Substitute.For<IApplicationContext>();
            var model = new LicenseViewModel(service, ctx);

            var exception = new Exception("Exception");
            var validation = new[]
            {
                new GeneralValidationFailure
                {
                    Message = "Message",
                    HowToResolve = "HowToResolve"
                }
            };
            var licenseResult = new LicenseResult(null, exception, validation);
            service.RegisterAsync(Arg.Any<Guid>()).Returns(licenseResult);
            service.ValidateAsync().Returns(licenseResult);

            model.Initialize();

            var key = new Guid("ae8cdf5f-26b3-4e2f-8e68-6ecc2e73720f");
            model.RegisterKey = key.ToString();

            model.ActivateCommand.Execute(null);

            service.Received().RegisterAsync(key);
            ctx.DidNotReceive().SetLicenseKeyAsync(Arg.Any<string>());
            Assert.Equal(AppLicense.Demo, LicenseGlobals.Get());
            Assert.Equal("|Message|HowToResolve|Exception|", model.ErrorMessage.Replace("\r\n", "|"));
            Assert.True(model.ShowError);
            Assert.False(model.IsBusy);
        }

        [Fact]
        public void ErrorMessageTest()
        {
            var service = Substitute.For<ILicenseService>();
            var ctx = Substitute.For<IApplicationContext>();
            var model = new LicenseViewModel(service, ctx);

            model.RegisterKey = "fake";

            Assert.Equal("Invalid license key.", model.ErrorMessage);
            Assert.True(model.ShowError);
        }
    }
}
