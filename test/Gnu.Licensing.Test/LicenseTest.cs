﻿using Gnu.Licensing;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using Xunit;
using System.IO;
using System.Text;
using Gnu.Licensing.Validation;
using System.Threading.Tasks;
using System.Net.Http;
using System.Globalization;

namespace Gnu.Licensing.Test
{
    public class LicenseTest : BaseLicenseTest
    {
        public LicenseTest()
        {
            SetupCertificate();
        }

        [Fact]
        public void Can_Generate_And_Validate_Signature_With_Standard_License()
        {
            var license = License.New()
                       .WithUniqueIdentifier(Guid.NewGuid())
                       .As(LicenseType.Standard)
                       .ExpiresAt(DateTime.MaxValue)
                       .WithMaximumUtilization(1)
                       .WithProductFeatures(new Dictionary<string, string>
                           {
                            {"Sales Module", "yes"},
                            {"Purchase Module", "yes"},
                            {"Maximum Transactions", "10000"}
                           })
                       .LicensedTo("Ferdinand Lukasak", "ferdinand.lukasak@example.com")
                       .WithAdditionalAttributes(new Dictionary<string, string>
                       {
                           { "Version", "1.0.0.0" }
                       })
                       .CreateAndSign(CertificateSearch);

            Assert.NotNull(license);
            Assert.NotNull(license.Signature);
            var xmlElement = XElement.Parse(license.ToString(), LoadOptions.None);
            Assert.True(xmlElement.Elements().Any());
            Assert.True(license.Id != Guid.Empty);
            Assert.Equal(LicenseType.Standard, license.Type);
            Assert.Equal(1, license.Quantity);
            Assert.NotNull(license.ProductFeatures);
            Assert.NotNull(license.Customer);
            Assert.True(license.VerifySignature());
            Assert.Equal(ConvertToRfc1123(DateTime.MaxValue), license.ExpirationUtc);
            Assert.Equal("1.0.0.0", license.AdditionalAttributes.Get("Version"));
        }

        [Fact]
        public void Can_Generate_And_Validate_Signature_With_Empty_License()
        {
            var license = License.New()
                                 .CreateAndSign(CertificateSearch);

            Assert.NotNull(license);
            Assert.NotNull(license.Signature);
            var xmlElement = XElement.Parse(license.ToString(), LoadOptions.None);
            Assert.True(xmlElement.Elements().Any());

            Assert.True(license.Id == Guid.Empty);
            Assert.Equal(LicenseType.Trial, license.Type);
            Assert.Equal(0, license.Quantity);
            Assert.Null(license.ProductFeatures);
            Assert.Null(license.Customer);
            Assert.True(license.VerifySignature());
            Assert.Equal(ConvertToRfc1123(DateTime.MaxValue), license.ExpirationUtc);
        }

        [Fact]
        public void Can_Detect_Hacked_License()
        {
            var licenseId = Guid.NewGuid();
            var customerName = "Ferdinand Lukasak";
            var customerEmail = "max@mustermann.tld";
            var expirationDate = DateTime.UtcNow.AddYears(1);
            var productFeatures = new Dictionary<string, string>
                                      {
                                          {"Sales Module", "yes"},
                                          {"Purchase Module", "yes"},
                                          {"Maximum Transactions", "10000"}
                                      };

            var license = License.New()
                                 .WithUniqueIdentifier(licenseId)
                                 .As(LicenseType.Standard)
                                 .WithMaximumUtilization(10)
                                 .WithProductFeatures(productFeatures)
                                 .LicensedTo(customerName, customerEmail)
                                 .ExpiresAt(expirationDate)
                                 .CreateAndSign(CertificateSearch);

            Assert.NotNull(license);
            Assert.NotNull(license.Signature);
            Assert.True(license.VerifySignature());

            // validate xml
            var xmlElement = XElement.Parse(license.ToString(), LoadOptions.None);
            Assert.True(xmlElement.Elements().Any());

            // manipulate xml
            Assert.NotNull(xmlElement.Element("Quantity"));
            xmlElement.Element("Quantity").Value = "11"; // now we want to have 11 licenses

            // load license from manipulated xml
            var hackedLicense = License.Load(xmlElement.ToString());

            // verify signature
            Assert.False(hackedLicense.VerifySignature());

            Assert.NotNull(hackedLicense);
            Assert.NotNull(hackedLicense.Signature);
            Assert.Equal(LicenseType.Standard, hackedLicense.Type);
            Assert.Equal(11, hackedLicense.Quantity);
            Assert.NotNull(hackedLicense.ProductFeatures);
            Assert.NotNull(hackedLicense.Customer);
            Assert.Equal(customerName, hackedLicense.Customer.Name);
            Assert.Equal(customerEmail, hackedLicense.Customer.Email);
            Assert.Equal(customerName, hackedLicense.Customer.Name);
            Assert.Equal(customerEmail, hackedLicense.Customer.Email);
            Assert.Equal(ConvertToRfc1123(expirationDate), hackedLicense.ExpirationUtc);
        }

        [Fact]
        public void ValidateLicenseTest()
        {
            var dir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var path = Path.Combine(dir, $"{nameof(ValidateLicenseTest)}.xml");

            try
            {
                var license = License.New()
                     .WithUniqueIdentifier(Guid.NewGuid())
                     .As(LicenseType.Trial)
                     .ExpiresAt(DateTime.UtcNow.AddDays(10))
                     .WithMaximumUtilization(1)
                     .LicensedTo(nameof(ValidateLicenseTest), $"{nameof(ValidateLicenseTest)}k@example.com")
                     .WithAdditionalAttributes(new Dictionary<string, string>
                     {
                           { "Version", "1.0.0.0" }
                     })
                     .CreateAndSign(CertificateSearch);

                using (var fileStream = File.Create(path))
                {
                    license.Save(fileStream);
                }

                License actual;
                using (var stream = File.OpenRead(path))
                {
                    actual = License.Load(stream);
                }

                var validationFailures = actual.Validate()
                                            .ExpirationDate()
                                            .When(lic => lic.Type == LicenseType.Trial)
                                            .And()
                                            .Signature()
                                            .AssertValidLicense().ToList();

                Assert.False(validationFailures.Any());
            }
            finally
            {
                RobustDelete(path);
            }
        }

        [Fact]
        public void ValidateLicense_InvalidTest()
        {
            var dir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var path = Path.Combine(dir, $"{nameof(ValidateLicense_InvalidTest)}.xml");

            try
            {
                var license = License.New()
                     .WithUniqueIdentifier(Guid.NewGuid())
                     .As(LicenseType.Trial)
                     .ExpiresAt(DateTime.UtcNow.AddDays(10))
                     .WithMaximumUtilization(1)
                     .LicensedTo(nameof(ValidateLicenseTest), $"{nameof(ValidateLicenseTest)}k@example.com")
                     .WithAdditionalAttributes(new Dictionary<string, string>
                     {
                           { "Version", "1.0.0.0" }
                     })
                     .CreateAndSign(CertificateSearch);

                using (var fileStream = File.Create(path))
                {
                    license.Save(fileStream);
                }

                License actual;
                using (var stream = File.OpenRead(path))
                {
                    actual = License.Load(stream);
                }

                var failure = new LicenseExpiredValidationFailure()
                {
                    Message = "Invalid version",
                    HowToResolve = "This is test only"
                };

                var validationFailures = actual.Validate()
                                            .ExpirationDate()
                                            .When(lic => lic.Type == LicenseType.Standard)
                                            .And()
                                            .Signature()
                                            .And()
                                            .AssertThat(x => x.AdditionalAttributes.Get("Version") == "1.0.0.1", failure)
                                            .AssertValidLicense().ToList();

                Assert.True(validationFailures.Any());
            }
            finally
            {
                RobustDelete(path);
            }
        }

        [Fact]
        public void DateParseTest()
        {
            var str = "Fri, 31 Dec 9999 23:59:59 GMT";

            var date = DateTime.ParseExact(str, "r", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
        }
    }
}