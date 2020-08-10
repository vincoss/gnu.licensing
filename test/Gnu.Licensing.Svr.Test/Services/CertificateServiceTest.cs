using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Gnu.Licensing.Svr.Services
{
    public class CertificateServiceTest
    {
        [Fact]
        public void CanInstall()
        {
            var dir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var path = Path.Combine(dir, "data", "Gnu.Licensing.pfx.txt");
            var pwd = "Pass@word1";
            var thumbprint = "947bf5472c8fd3d11d6227b2bbba589cd60e3973";
            var service = new CertificateService();
            service.Install(path, pwd, thumbprint);
        }
    }
}
