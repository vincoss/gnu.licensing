using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Xunit;

namespace Gnu.Licensing.Test
{
    public class XmlExtensionsTest : BaseLicenseTest
    {
        /// <summary>
        /// https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.xml.signedxml.signature?view=dotnet-plat-ext-3.1
        /// </summary>
        [Fact]
        public void SignXml()
        {
            var path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var dataPath = Path.Combine(path, "Data", "test.xml");

            try
            {
                SetupCertificate();

                new XElement(nameof(SignXml)).Save(dataPath);
                var xmlDoc = new XmlDocument();
                xmlDoc.PreserveWhitespace = true;
                xmlDoc.Load(dataPath);

                var cert = XmlExtensions.GetCertificate("CN=Gnu.Licensing", StoreName.My, StoreLocation.CurrentUser);
                XmlExtensions.SignXml(xmlDoc, cert);

                bool result = XmlExtensions.VerifyXmlFile(xmlDoc);

                Assert.True(result);
            }
            finally
            {
                File.Delete(dataPath);
            }
        }
    }
}
