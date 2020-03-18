using System;
using samplesl.Validation;


namespace samplesl
{
    public class LicenseRegisterResult
    {
        public string License { get; set; }
        public GeneralValidationFailure Failure { get; set; }

    }
}
