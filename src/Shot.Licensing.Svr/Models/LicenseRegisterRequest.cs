using System;
using System.Collections.Generic;


namespace Shot.Licensing.Svr.Models
{
    public class LicenseRegisterRequest
    {
        public Guid LicenseId { get; set; }
        public Guid ProductId { get; set; }
        public IDictionary<string, string> Attributes { get; set; }
    }
}
