using Standard.Licensing;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shot.Licensing.Cli
{
    /// <summary>
    /// dotnet Shot.Licensing.Cli.dll keys
    /// dotnet Shot.Licensing.Cli.dll keys -s 512 -d c:\temp\lic
    /// </summary>
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var program = new Program();
            var cmd = new RootCommand();
            cmd.AddCommand(program.Keys());
            cmd.AddCommand(program.Lic());
            return await cmd.InvokeAsync(args);
        }

        private Command Keys()
        {
            var cmd = new Command("keys", "Create public and private keys with the specified key size.");
            cmd.AddOption(new Option(new[] { "--size", "-s" }, "The key size, in bits.")
            {
                Argument = new Argument<int>(() => Constants.DefaultKeySize)
                {
                    Arity = ArgumentArity.ExactlyOne,
                }
            });
            cmd.AddOption(new Option(new[] { "--directory", "-d" }, "Output directory.")
            {
                Argument = new Argument<DirectoryInfo>(() => GetDefaultDirectory())
                {
                    Arity = ArgumentArity.ExactlyOne,
                }
            });
            cmd.Handler = CommandHandler.Create<int, DirectoryInfo>((size, directory) =>
            {
                if (size <= 0)
                {
                    Console.WriteLine("Invalid key size.");
                    return 1;
                }

                if (directory.Exists == false)
                {
                    Console.WriteLine("Creating directory.");

                    directory.Create();
                }

                GenerateKeys(size, directory.FullName);

                Console.WriteLine("Created keys.");

                return 0;
            });
            return cmd;
        }

        private Command Lic()
        {
            var cmd = new Command("li", "Create.");
            cmd.AddOption(new Option(new[] { "--name", "-n" }, "Licensed to name.")
            {
                Argument = new Argument<string>()
                {
                    Arity = ArgumentArity.ExactlyOne,
                }
            });
            cmd.AddOption(new Option(new[] { "--email", "-e" }, "Licensed to email.")
            {
                Argument = new Argument<string>()
                {
                    Arity = ArgumentArity.ExactlyOne,
                }
            });
            cmd.AddOption(new Option(new[] { "--type", "-t" }, $"License type. Default: {LicenseType.Standard}. Available types: <{LicenseType.Trial},{LicenseType.Standard}>")
            {
                Argument = new Argument<LicenseType>(() => LicenseType.Standard)
                {
                    Arity = ArgumentArity.ExactlyOne,
                }
            });
            cmd.AddOption(new Option(new[] { "--expire", "-x" }, "License expire date. Default never.")
            {
                Argument = new Argument<DateTime>(() => DateTime.MaxValue)
                {
                    Arity = ArgumentArity.ExactlyOne,
                }
            });
            cmd.AddOption(new Option(new[] { "--volume", "-v" }, "Maximum utilization of the license. Default 1.")
            {
                Argument = new Argument<int>(() => 1)
                {
                    Arity = ArgumentArity.ExactlyOne,
                }
            });
            cmd.AddOption(new Option(new[] { "--features", "-f" }, "Product features.")
            {
                Argument = new Argument<string>()
                {
                    Arity = ArgumentArity.ZeroOrMore,
                }
            });
            cmd.AddOption(new Option(new[] { "--attributes", "-a" }, "Additional attributes.")
            {
                Argument = new Argument<string>()
                {
                    Arity = ArgumentArity.ZeroOrMore,
                }
            });
            cmd.Handler = CommandHandler.Create<IEnumerable<string>>((additional) =>
            {
                if (additional == null)
                {
                    Console.WriteLine("Invalid key size.");
                    return 1;
                }

                var aditionalAttributes = AddVariable(additional);

                foreach (var v in aditionalAttributes)
                {
                    Console.WriteLine($"{v.Key}--{v.Value}");
                }


                Generate();

                return 0;
            });
            return cmd;
        }

        private static IDictionary<string, string> AddVariable(IEnumerable<string> args)
        {
            var variables = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (args == null)
            {
                return variables;
            }
            foreach (var pair in args)
            {
                if (pair.IndexOf('=') == -1)
                {
                    Console.WriteLine("Please enter correct form of variable name and variable value. key=value");
                }
                string[] array = pair.Split(new char[] { '=' });
                variables[array[0]] = ((array.Length > 1) ? array[1] : "");
            }
            return variables;
        }

        private static DirectoryInfo GetDefaultDirectory()
        {
            var path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            return new DirectoryInfo(path);
        }

        private void GenerateKeys(int keySize, string directory)
        {
            using (var rsa = RSA.Create())
            {
                rsa.KeySize = keySize;
                var pub = rsa.ToXmlString(false);
                var pri = rsa.ToXmlString(true);
                var name = $"{DateTime.Now.ToString("yyyyMMddHHmmss")}";

                var pubf = Path.Combine(directory, $"{name}.public.xml");
                var prif = Path.Combine(directory, $"{name}.private.xml");

                WriteToFile(pubf, pub);
                WriteToFile(prif, pri);
            }
        }

        private async void WriteToFile(string filePath, string content)
        {
            using (StreamWriter writer = File.CreateText(filePath))
            {
                await writer.WriteLineAsync(content);
            }
        }

        private void Generate(string toName, string toEmail, LicenseType type, DateTime expire, int volume, IDictionary<string, string> features, IDictionary<string, string> attributes, Stream pkStream, Stream destination)
        {
            string pss = null;
            using (var reader = new StreamReader(pkStream))
            {
                pss = reader.ReadToEnd();
            }

            var license = License.New()
                       .WithUniqueIdentifier(Guid.NewGuid())
                       .As(type)
                       .ExpiresAt(expire)
                       .WithMaximumUtilization(volume)
                       .WithProductFeatures(features)
                       .WithAdditionalAttributes(attributes)
                       .LicensedTo(toName, toEmail)
                       .CreateAndSignWithPrivateKey(pss);

            var xmlElement = XElement.Parse(license.ToString(), LoadOptions.None);
            xmlElement.Save(destination);
        }
    }
}