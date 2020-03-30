using Shot.Licensing.Validation;


namespace Shot.Licensing.Svr.Models
{
    public class LicenseRegisterResponse
    {
        public string License { get; set; }
        public IValidationFailure Failure { get; set; }

    }
}
