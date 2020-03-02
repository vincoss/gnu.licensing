using System;

namespace Shot.Licensing.Sample_Console_ServerLicenseFetch
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        public async void Sample()
        {
            /* 
                1. Assume that the user already paid for the license and received an license ID (GUID|UUID). 
                2. Call license service to generate actual license.
                3. Pass additional parameters to lock the license to a particular machine or software installation. The passed parameters will be added into the license file.
                4. Save license into client device
                5. Validate lincese on the client device
            */

            throw new NotSupportedException();
        }
    }
}
