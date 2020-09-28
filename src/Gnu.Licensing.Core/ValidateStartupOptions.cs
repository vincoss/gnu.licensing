using Gnu.Licensing.Core.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;


namespace Gnu.Licensing.Core
{
    public class ValidateStartupOptions
    {
        private readonly IOptions<LicensingOptions> _root;
        private readonly IOptions<DatabaseOptions> _database;
        private readonly ILogger<ValidateStartupOptions> _logger;

        public ValidateStartupOptions(IOptions<LicensingOptions> root, IOptions<DatabaseOptions> database, ILogger<ValidateStartupOptions> logger)
        {
            _root = root ?? throw new ArgumentNullException(nameof(root));
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public bool Validate()
        {
            try
            {
                _ = _root.Value;
                _ = _database.Value;
                return true;
            }
            catch (OptionsValidationException e)
            {
                foreach (var failure in e.Failures)
                {
                    _logger.LogError("{OptionsFailure}", failure);
                }

                _logger.LogError(e, "Configuration is invalid.");
                return false;
            }
        }
    }
}
