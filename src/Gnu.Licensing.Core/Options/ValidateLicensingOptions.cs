using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Gnu.Licensing.Core.Options
{
    public class ValidateLicensingOptions<TOptions> : IValidateOptions<TOptions> where TOptions : class
    {
        private readonly string _optionsName;

        public ValidateLicensingOptions(string optionsName)
        {
            _optionsName = optionsName;
        }

        public ValidateOptionsResult Validate(string name, TOptions options)
        {
            var context = new ValidationContext(options);
            var validationResults = new List<ValidationResult>();
            if (Validator.TryValidateObject(options, context, validationResults, validateAllProperties: true))
            {
                return ValidateOptionsResult.Success;
            }

            var errors = new List<string>();
            var message = (_optionsName == null)
                ? $"Invalid configs"
                : $"Invalid '{_optionsName}' configs";

            foreach (var result in validationResults)
            {
                errors.Add($"{message}: {result}");
            }

            return ValidateOptionsResult.Fail(errors);
        }
    }
}