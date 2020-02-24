using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

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
            return await cmd.InvokeAsync(args);
        }

        private  Command Keys()
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

                if(directory.Exists == false)
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
    }
}