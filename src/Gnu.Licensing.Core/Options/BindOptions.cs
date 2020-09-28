using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;


namespace Gnu.Licensing.Core.Options
{
    public class BindOptions<TOptions> : IConfigureOptions<TOptions> where TOptions : class
    {
        private readonly IConfiguration _config;

        public BindOptions(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public void Configure(TOptions options) => _config.Bind(options);
    }
}
