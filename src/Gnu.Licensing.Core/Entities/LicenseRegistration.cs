using System;
using System.ComponentModel.DataAnnotations;


namespace Gnu.Licensing.Core.Entities
{
    public class LicenseRegistration
    {
        [Key]
        public Guid LicenseRegistrationId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public Guid CompanyId { get; set; }

        [Required]
        public string LicenseName { get; set; }

        [Required]
        public string LicenseEmail { get; set; }

        [Required]
        public LicenseType LicenseType { get; set; }

        [Required]
        public bool IsActive { get; set; }  // Note: To blacklist

        public string Comment { get; set; } // Note: Reasons for blacklist

        [Required]
        public int Quantity { get; set; }

        public DateTime? ExpireUtc { get; set; }  

        [Required]
        public DateTime CreatedDateTimeUtc { get; set; }

        [Required]
        public string CreatedByUser { get; set; }
    }
}
