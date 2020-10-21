using System;
using System.ComponentModel.DataAnnotations;


namespace Gnu.Licensing.Core.Entities
{
    public class LicenseActivation
    {
        [Key]
        public Guid LicenseActivationId { get; set; }

        [Required]
        public Guid LicenseId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public Guid CompanyId { get; set; }

        [Required]
        public string LicenseString { get; set; }

        public string LicenseAttributes { get; set; }

        [Required]
        public string LicenseChecksum { get; set; }

        public string AttributesChecksum { get; set; }

        [Required]
        public string ChecksumType { get; set; }

        [Required]
        public DateTime CreatedDateTimeUtc { get; set; }

        [Required]
        public string CreatedByUser { get; set; }
    }
}
