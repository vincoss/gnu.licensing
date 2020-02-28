using Shot.Licensing.Cli;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Shot.Licensing.Test
{
    public class ArgsServiceTest
    {
        [Fact]
        public async void KeysTest()
        {
            var dir = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, nameof(KeysTest));

            try
            {
                var args = new[]
                {
                    "keys", "-s", "512", "-d", dir
                };
                var service = new ArgsService();
                var result = await service.Run(args);

                Assert.Equal(0, result);
            }
            finally
            {
                Directory.Delete(dir, true);
            }
        }

        [Fact]
        public async void KeysTest_False()
        {
            var dir = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, nameof(KeysTest_False));

            var args = new[]
                {
                    "keys", "-s", "511", "-d", dir
                };
            var service = new ArgsService();
            var result = await service.Run(args);

            Assert.Equal(1, result);
        }

        [Fact]
        public async void LicenseTest()
        {
            var dir = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, nameof(LicenseTest));

            try
            {
                if(Directory.Exists(dir) == false)
                {
                    Directory.CreateDirectory(dir);
                }

                // generate keys
                XmlExtensions.GenerateKeys(Constants.DefaultKeySize, dir);
                var signKey = Directory.GetFiles(dir).Single(x => x.EndsWith("private.xml"));

                var args = new[]
                {
                    "license", "-n", "test-license", "-e", "asd@example.com", "-t", "standard", 
                    "-x", DateTime.Now.AddYears(1).ToString("yyyy-MM-dd"), "-v", "5",
                    "-f", "f1=v1", "-f", "-f2=v2",
                    "-a", "a1=v1", "-a", "a2=a2",
                    "-d", dir,
                    "-s", signKey
                };
                var service = new ArgsService();
                var result = await service.Run(args);

                Assert.Equal(0, result);
            }
            finally
            {
                Directory.Delete(dir, true);
            }
        }

        [Fact]
        public async void LicenseTest_False()
        {
            var dir = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, nameof(LicenseTest_False));

            try
            {
                if (Directory.Exists(dir) == false)
                {
                    Directory.CreateDirectory(dir);
                }

                // generate keys
                XmlExtensions.GenerateKeys(Constants.DefaultKeySize, dir);
                var signKey = Directory.GetFiles(dir).Single(x => x.EndsWith("private.xml"));

                var args = new[]
                {
                    "license", "-n", "-e", "asd@example.com", "-t", "standard",
                    "-x", DateTime.Now.AddYears(1).ToString("yyyy-MM-dd"), "-v", "5",
                    "-f", "f1=v1", "-f", "-f2=v2",
                    "-a", "a1=v1", "-a", "a2=a2",
                    "-d", dir,
                    "-s", signKey
                };
                var service = new ArgsService();
                var result = await service.Run(args);

                Assert.Equal(1, result);
            }
            finally
            {
                Directory.Delete(dir, true);
            }
        }
    }
}
