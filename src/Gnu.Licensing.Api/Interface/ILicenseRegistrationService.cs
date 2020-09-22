﻿using Gnu.Licensing.Api.ViewModels;
using System;
using System.Threading.Tasks;


namespace Gnu.Licensing.Api.Interface
{
    public interface ILicenseRegistrationService
    {
        Task<Guid> CreateAsync(LicenseRegistrationViewModel model, string createdByUser);
    }
}
