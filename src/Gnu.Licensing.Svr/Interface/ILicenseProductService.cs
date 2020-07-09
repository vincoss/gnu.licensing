﻿using Gnu.Licensing.Svr.ViewModels;
using System;
using System.Threading.Tasks;


namespace Gnu.Licensing.Svr.Interface
{
    public interface ILicenseProductService
    {
        Task<Guid> Create(LicenseProductViewModel model, string createdByUser);
    }
}
