//
// Copyright © 2003-2020 https://github.com/vincoss/Gnu.Licensing
//
// Author:
//  Ferdinand Lukasak
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using System.Xml.Linq;


namespace Gnu.Licensing
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/dotnet/standard/security/how-to-verify-the-digital-signatures-of-xml-documents
    /// </summary>
    public static class XmlExtensions
    {
        public static XmlDocument ToXmlDocument(this XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }
            var doc = new XmlDocument();
            doc.Load(element.CreateReader());
            return doc;
        }

        //public static XmlElement ToXmlElement(this XElement element)
        //{
        //    return ToXmlDocument(element).DocumentElement;
        //}

        public static XElement ToXElement(this XmlElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }
            return XElement.Parse(element.OuterXml);
        }

        //public static void SignXml(this XmlDocument xmlDoc, RSA key)
        //{
        //    if (xmlDoc == null)
        //    {
        //        throw new ArgumentException(nameof(xmlDoc));
        //    }
        //    if (key == null)
        //    {
        //        throw new ArgumentException(nameof(key));
        //    }

        //    // Get the XML representation of the signature and save it to an XmlElement object.
        //    XmlElement xmlDigitalSignature = GetXmlDigitalSignature(xmlDoc, key);

        //    // Append the element to the XML document.
        //    xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));
        //}

        //public static bool VerifyXml(this XmlDocument xmlDoc, RSA key)
        //{
        //    if (xmlDoc == null)
        //    {
        //        throw new ArgumentException(nameof(xmlDoc));
        //    }
        //    if (key == null)
        //    {
        //        throw new ArgumentException(nameof(key));
        //    }

        //    var signedXml = new SignedXml(xmlDoc);
        //    var nodeList = xmlDoc.GetElementsByTagName("Signature");

        //    // Throw an exception if no signature was found.
        //    if (nodeList.Count <= 0)
        //    {
        //        throw new CryptographicException("Verification failed: No Signature was found in the document.");
        //    }

        //    // Throw an exception if more than one signature was found.
        //    if (nodeList.Count >= 2)
        //    {
        //        throw new CryptographicException("Verification failed: More that one signature was found for the document.");
        //    }

        //    // Load the first <signature> node.  
        //    signedXml.LoadXml((XmlElement)nodeList[0]);

        //    // Check the signature and return the result.
        //    return signedXml.CheckSignature(key);
        //}

        //public static XmlElement GetXmlDigitalSignature(this XmlDocument xmlDoc, RSA key)
        //{
        //    if (xmlDoc == null)
        //    {
        //        throw new ArgumentException(nameof(xmlDoc));
        //    }
        //    if (key == null)
        //    {
        //        throw new ArgumentException(nameof(key));
        //    }

        //    var signedXml = new SignedXml(xmlDoc);
        //    signedXml.SigningKey = key;

        //    // Create a reference to be signed.
        //    var reference = new Reference();
        //    reference.Uri = "";

        //    // Add an enveloped transformation to the reference.
        //    var env = new XmlDsigEnvelopedSignatureTransform();
        //    reference.AddTransform(env);

        //    // Add the reference to the SignedXml object.
        //    signedXml.AddReference(reference);

        //    // Compute the signature.
        //    signedXml.ComputeSignature();

        //    // Get the XML representation of the signature.
        //    return signedXml.GetXml();
        //}

        //public static void GenerateKeys(int keySize, string directory)
        //{
        //    if(keySize <= 0)
        //    {
        //        throw new ArgumentException(nameof(keySize));
        //    }
        //    if(string.IsNullOrWhiteSpace(directory))
        //    {
        //        throw new ArgumentNullException(nameof(directory));
        //    }

        //    using (var rsa = RSA.Create())
        //    {
        //        rsa.KeySize = keySize;
        //        var pub = rsa.ToXmlString(false);
        //        var pri = rsa.ToXmlString(true);
        //        var name = $"{DateTime.Now.ToString("yyyyMMddHHmmss")}";

        //        var pubf = Path.Combine(directory, $"{name}.public.xml");
        //        var prif = Path.Combine(directory, $"{name}.private.xml");
                
        //        using (StreamWriter writer = File.CreateText(pubf))
        //        {
        //            var xpub = XElement.Parse(pub);
        //            xpub.Save(writer);
        //        }

        //        using (StreamWriter writer = File.CreateText(prif))
        //        {
        //            var xpub = XElement.Parse(pri);
        //            xpub.Save(writer);
        //        }
        //    }
        //}

        //public static void Generate(string toName, string toEmail, LicenseType type, DateTime expire, int volume, IDictionary<string, string> features, IDictionary<string, string> attributes, FileInfo pkInfo, DirectoryInfo destination)
        //{
        //    if(string.IsNullOrWhiteSpace(toName))
        //    {
        //        throw new ArgumentNullException(nameof(toName));
        //    }
        //    if (string.IsNullOrWhiteSpace(toEmail))
        //    {
        //        throw new ArgumentNullException(nameof(toEmail));
        //    }
        //    if(expire <= DateTime.Now)
        //    {
        //        throw new ArgumentException(nameof(expire));
        //    }
        //    if(volume <= 0)
        //    {
        //        throw new ArgumentException(nameof(volume));
        //    }
        //    if(features == null)
        //    {
        //        features = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        //    }
        //    if (attributes == null)
        //    {
        //        attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        //    }
        //    if(pkInfo == null)
        //    {
        //        throw new ArgumentNullException(nameof(pkInfo));
        //    }
        //    if (destination == null)
        //    {
        //        throw new ArgumentNullException(nameof(destination));
        //    }

        //    string pss = null;
        //    using (var reader = new StreamReader(pkInfo.OpenRead()))
        //    {
        //        pss = reader.ReadToEnd();
        //    }

        //    var license = Gnu.Licensing.License.New()
        //               .WithUniqueIdentifier(Guid.NewGuid())
        //               .As(type)
        //               .ExpiresAt(expire)
        //               .WithMaximumUtilization(volume)
        //               .WithProductFeatures(features)
        //               .WithAdditionalAttributes(attributes)
        //               .LicensedTo(toName, toEmail)
        //               .CreateAndSignWithPrivateKey(pss);

        //    var fileName = $"{license.Id}.xml";
        //    var path = Path.Combine(destination.FullName, fileName);
        //    using (var writer = File.CreateText(path))
        //    {
        //        var xmlElement = XElement.Parse(license.ToString(), LoadOptions.None);
        //        xmlElement.Save(writer);
        //    }
        //}



        public static void SignXml(XmlDocument document, X509Certificate2 certificate)
        {
            if(document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }
            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }
            if (certificate.HasPrivateKey == false)
            {
                throw new InvalidOperationException($"Certificate does not have private key. {certificate}");
            }

            var xmlDigitalSignature = GetXmlSignature(document, certificate);

            // Append the element to the XML document.
            document.DocumentElement.AppendChild(document.ImportNode(xmlDigitalSignature, true));

            if (document.FirstChild is XmlDeclaration)
            {
                document.RemoveChild(document.FirstChild);
            }
        }

        public static XmlElement GetXmlSignature(this XmlDocument document, X509Certificate2 certificate)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }
            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate));
            }
            if (certificate.HasPrivateKey == false)
            {
                throw new InvalidOperationException($"Certificate does not have private key. {certificate}");
            }

            var key = certificate.PrivateKey;

            var signedXml = new SignedXml(document);
            signedXml.SigningKey = key;

            var XMLSignature = signedXml.Signature;

            // Create a reference to be signed.  Pass "" to specify that all of the current XML document should be signed.
            var reference = new Reference("");

            // Add an enveloped transformation to the reference.
            var env = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(env);

            // Add the Reference object to the Signature object.
            XMLSignature.SignedInfo.AddReference(reference);

            // Add an RSAKeyValue KeyInfo (optional; helps recipient find key to validate).
            var keyInfo = new KeyInfo();
            keyInfo.AddClause(new RSAKeyValue((RSA)key));

            // Add the KeyInfo object to the Reference object.
            XMLSignature.KeyInfo = keyInfo;

            // Compute the signature.
            signedXml.ComputeSignature();

            // Get the XML representation of the signature and save
            // it to an XmlElement object.
            var xmlDigitalSignature = signedXml.GetXml();

            return xmlDigitalSignature;
        }

        public static bool VerifyXmlFile(XmlDocument doc)
        {
            if (doc == null)
            {
                throw new ArgumentNullException(nameof(doc));
            }

            // Create a new SignedXml object and pass it the XML document class.
            var signedXml = new SignedXml(doc);

            // Find the "Signature" node and create a new XmlNodeList object.
            var nodeList = doc.GetElementsByTagName("Signature");

            // Load the signature node.
            signedXml.LoadXml((XmlElement)nodeList[0]);

            // Check the signature and return the result.
            return signedXml.CheckSignature();
        }

        public static X509Certificate2 GetCertificate(string signingCredentialSearch, StoreName storeName = StoreName.Root, StoreLocation storeLocation = StoreLocation.LocalMachine)
        {
            if (string.IsNullOrWhiteSpace(signingCredentialSearch))
            {
                throw new ArgumentNullException(nameof(signingCredentialSearch));
            }

            using (var store = new X509Store(storeName, storeLocation))
            {
                store.Open(OpenFlags.ReadOnly);
                var cert = store.Certificates.OfType<X509Certificate2>().AsEnumerable()
                    .FirstOrDefault(c => c.Subject.IndexOf(signingCredentialSearch, StringComparison.OrdinalIgnoreCase) >= 0);

                if(cert == null)
                {
                    throw new InvalidOperationException($"Could not find certificate. Search: {signingCredentialSearch}, StoreName: {storeName}, StoreLocation: {storeLocation}");
                }
                return cert;
            }
        }
    }
}
