﻿using System;
using Gnu.Licensing.Sample_Console_ServerLicenseFetch.Services;


namespace Gnu.Licensing.Sample_Console_ServerLicenseFetch
{
    /// <summary>
    /// NOTE: Ensure that the 'Gnu.Licensing.Api' service is running.
    /// </summary>
    class Program
    {
        public static readonly string MachineName = Environment.MachineName;

        static void Main(string[] args)
        {
            var p = new Program();
            p.Sample();

            Console.WriteLine("Done...");
            Console.Read();
        }

        public async void Sample()
        {
            /* 
                1. Assume that the user|customer already paid for the license and received an license ID (GUID|UUID). 
                2. Call license service to generate|activate actual license.
                3. Pass additional parameters to lock the license to a particular machine or software installation. The passed parameters will be added into the license file and signed.
                4. Save license into client device
                5. Validate lincese on the client device
            */

            // User got license from purchase or regisration.
            var licenseId = new Guid("D65321D5-B0F9-477D-828A-086F30E2BF89");

            // Get a license file from the server.
            var service = new LicenseService(BaseLicenseService.CreateHttpClient());
            var result = await service.RegisterAsync(licenseId);

            if(result.Successful)
            {
                Console.WriteLine(service.GetPath());
                Console.WriteLine("Device is licensed. (Full)");
            }
            else
            {
                foreach (var f in result.Failures)
                {
                    Console.WriteLine(f.Code);
                    Console.WriteLine(f.Message);
                    Console.WriteLine(f.HowToResolve);
                }

                if (result.Exception != null)
                {
                    Console.WriteLine(result.Exception);
                }
            }
        }
    }
}
