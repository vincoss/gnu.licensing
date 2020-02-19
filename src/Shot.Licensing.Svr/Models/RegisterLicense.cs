using System;
using System.Collections.Generic;


namespace Shot.Licensing.Svr.Models
{
    public class RegisterLicense
    {
        public Guid LicenseId { get; set; }
        public IDictionary<string, string> Attributes { get; set; }
    }
}
