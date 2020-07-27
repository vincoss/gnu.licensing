using Gnu.Licensing;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;


namespace Gnu.Licensing.Cli
{
    public class ArgsService
    {
        public Task<int> Run(string[] args)
        {
            if(args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }
            var cmd = new RootCommand();
            cmd.AddCommand(License());
            return cmd.InvokeAsync(args);
        }

        private Command License()
        {
            var cmd = new Command("license", "Create license.");
            
            cmd.AddOption(new Option(new[] { "--license key", "-k" }, "License key.")
            {
                Argument = new Argument<Guid>()
                {
                    Arity = ArgumentArity.ExactlyOne,
                }
            });
            cmd.AddOption(new Option(new[] { "--additional", "-a" }, "Additional attributes.")
            {
                Argument = new Argument<string>()
                {
                    Arity = ArgumentArity.ZeroOrMore,
                }
            });
            cmd.AddOption(new Option(new[] { "--directory", "-d" }, "Output directory.")
            {
                Argument = new Argument<DirectoryInfo>(() => GetDefaultDirectory())
                {
                    Arity = ArgumentArity.ExactlyOne,
                }
            });
            cmd.Handler = CommandHandler.Create<LicenseArgs>((args) =>
            {
                var aditionalAttributes = AddVariable(args.Additional ?? Array.Empty<string>());

              //  XmlExtensions.Generate(args.LicenseId, aditionalAttributes, args.Directory);

                Console.WriteLine("Created license.");

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
    }
}
