using samplesl.Validation;


namespace samplesl.Svr.Models
{
    public class LicenseRegisterResponse
    {
        public string License { get; set; }
        public IValidationFailure Failure { get; set; }

    }
}
