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
            cmd.AddCommand(Keys());
            cmd.AddCommand(License());
            return cmd.InvokeAsync(args);
        }

        private Command Keys()
        {
            var cmd = new Command("keys", "Create public and private keys with the specified key size.");

            var sizeArgument = new Argument<int>(() => Constants.DefaultKeySize)
            {
                Arity = ArgumentArity.ExactlyOne,
            };

            sizeArgument.AddValidator(r =>
            {
                var value = r.GetValueOrDefault<int>();
                var message = ValidateKeySize(value);
                if(string.IsNullOrWhiteSpace(message))
                {
                    return null;
                }
                return $"Argument '{r.Argument.Name}' cannot be set to '{value}'. {message}";
            });

            cmd.AddOption(new Option(new[] { "--size", "-s" }, "The key size, in bits.")
            {
                Argument = sizeArgument
            });

            var directoryArgument = new Argument<DirectoryInfo>(() => GetDefaultDirectory())
            {
                Arity = ArgumentArity.ExactlyOne,
            };

            cmd.AddOption(new Option(new[] { "--directory", "-d" }, "Output directory.")
            {
                Argument = directoryArgument
            });

            cmd.Handler = CommandHandler.Create<KeysArgs>((args) =>
            {
                if (args.Directory.Exists == false)
                {
                    Console.WriteLine($"Creating directory. {args.Directory.FullName}");

                    args.Directory.Create();
                }

                XmlExtensions.GenerateKeys(args.Size, args.Directory.FullName);
                Console.WriteLine("Created keys.");

                return 0;
            });

            return cmd;
        }

        private Command License()
        {
            var cmd = new Command("license", "Create license.");
            
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
            cmd.AddOption(new Option(new[] { "--type", "-t" }, $"License type. Default: {LicenseType.Standard}. Available types: <{LicenseType.Trial},{LicenseType.Standard}>.")
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
            cmd.AddOption(new Option(new[] { "--additional", "-a" }, "Additional attributes.")
            {
                Argument = new Argument<string>()
                {
                    Arity = ArgumentArity.ZeroOrMore,
                }
            });
            cmd.AddOption(new Option(new[] { "--sign", "-s" }, "Sign key.")
            {
                Argument = new Argument<Stream>()
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
            cmd.Handler = CommandHandler.Create<LicenseArgs>((args) =>
            {
                var featureAttributes = AddVariable(args.Features ?? Array.Empty<string>());
                var aditionalAttributes = AddVariable(args.Additional ?? Array.Empty<string>());

                XmlExtensions.Generate(args.Name, args.Email, args.Type, args.Expire, args.Volume, featureAttributes, aditionalAttributes, args.Sign, args.Directory);

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

        public static string ValidateKeySize(int size)
        {
            string message = null;
            using (var rsa = RSA.Create())
            {
                var sizes = rsa.LegalKeySizes;
                if (sizes.Length > 0)
                {
                    var s = sizes[0];

                    if (size < s.MinSize || size > s.MaxSize || (size % s.SkipSize) != 0)
                    {
                        message = $"The key sizes that are supported. Min: {sizes[0].MinSize}. Max: {sizes[0].MaxSize}. Skip: {sizes[0].SkipSize}.";
                    }
                }
            }
            return message;
        }
    }
}
