using Shot.Licensing.Validation;


namespace Shot.Licensing.Api.Models
{
    public class LicenseRegisterResponse
    {
        public string License { get; set; }
        public IValidationFailure Failure { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(License))
            {
                return base.ToString();
            }
            return License;
        }
    }
}
