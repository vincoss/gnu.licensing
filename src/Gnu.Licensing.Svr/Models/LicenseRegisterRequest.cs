﻿using System;
using System.Collections.Generic;


namespace Gnu.Licensing.Svr.Models
{
    public class LicenseRegisterRequest
    {
        public Guid LicenseUuid { get; set; }
        public Guid ProductUuid { get; set; }
        public IDictionary<string, string> Attributes { get; set; }
    }
}
