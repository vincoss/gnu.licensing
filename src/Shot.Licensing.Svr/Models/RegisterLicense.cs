﻿using System;
using System.Collections.Generic;


namespace samplesl.Svr.Models
{
    public class RegisterLicense
    {
        public Guid LicenseId { get; set; }
        public IDictionary<string, string> Attributes { get; set; }
    }
}
