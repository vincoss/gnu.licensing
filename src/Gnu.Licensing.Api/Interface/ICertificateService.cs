using System;


namespace Gnu.Licensing.Api.Interface
{
    public interface ICertificateService
    {
        void Install(string path, string password, string thumbprint);
    }
}