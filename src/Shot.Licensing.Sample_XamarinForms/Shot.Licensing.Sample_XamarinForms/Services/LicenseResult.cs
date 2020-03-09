using System;
using System.Linq;
using System.Collections.Generic;
using samplesl.Validation;


namespace samplesl.Sample_XamarinForms.Services
{
    public sealed class LicenseResult
    {
        public LicenseResult(License license, Exception exception, IEnumerable<IValidationFailure> failures)
        {
            License = license;
            Successful = (exception == null && failures == null || (failures != null && failures.Any() == false));
            Exception = exception;
            Failures = failures;
        }

        public License License { get; private set; }

        public bool Successful { get; private set; }

        public Exception Exception { get; private set; }

        public IEnumerable<IValidationFailure> Failures { get; private set; }
    }
}
