﻿using System;
using System.Threading.Tasks;


namespace Gnu.Licensing
{
    public interface ILicenseService
    {
        string GetPath();

        Task<LicenseResult> RegisterAsync(Guid licenseKey);

        Task<LicenseResult> ValidateAsync(bool onlineCheck = false);

        Task RunAsync(bool onlineCheck = false);
    }
}
