using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Xunit;

namespace Shot.Licensing.Test
{
    public class XmlExtensionsTest : BaseLicenseTest
    {
        [Fact]
        public void GenerateKeys()
        {
            var key = RSA.Create(4096);
            var puk = key.ToXmlString(false);
            var pri = key.ToXmlString(true);

            var dir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var pukf = Path.Combine(dir, $@"C:\Temp\{nameof(GenerateKeys)}.puk.xml");
            var prif = Path.Combine(dir, $@"C:\Temp\{nameof(GenerateKeys)}.pri.xml");

            File.WriteAllText(pukf, puk, Encoding.UTF8);
            File.WriteAllText(prif, pri, Encoding.UTF8);
        }

        [Fact]
        public void SignXml()
        {
            var dir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var path = Path.Combine(dir, $"{nameof(SignXml)}.xml");

            try
            {
                new XElement("license").Save(path);
                var xmlDoc = new XmlDocument();
                xmlDoc.PreserveWhitespace = true;
                xmlDoc.Load(path);

                using (var rsaKey = RSA.Create())
                {
                    rsaKey.FromXmlString(PrivateKey);
                    xmlDoc.SignXml(rsaKey);
                    xmlDoc.Save(path);

                    var result = xmlDoc.VerifyXml(rsaKey);
                }

                using (var rsaKey = RSA.Create())
                {
                    rsaKey.FromXmlString(PublicKey);
                    var result = xmlDoc.VerifyXml(rsaKey);

                    Assert.True(result);
                }
            }
            finally
            {
                  File.Delete(path);
            }
        }
    }
}
