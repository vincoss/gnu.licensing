﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Warthog.Api.ViewModels
{
    public class LicenseRegistrationViewModel
    {
        public Guid ProductUuid { get; set; }
        public string LicenseName { get; set; }
        public string LicenseEmail { get; set; }
        public LicenseType LicenseType { get; set; }
        public int Quantity { get; set; }
    }
}
