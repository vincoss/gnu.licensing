using System;


namespace Gnu.Licensing.Svr.Interface
{
    public interface ICertificateService
    {
        void Install(string path, string password, string thumbprint);
    }
}