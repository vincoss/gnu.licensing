using System;
using System.ComponentModel.DataAnnotations;


namespace Gnu.Licensing.Svr.Models
{
    public class CertificateInstallRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Thumbprint { get; set; }
    }
}
