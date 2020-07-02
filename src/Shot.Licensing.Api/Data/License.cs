using System;
using System.ComponentModel.DataAnnotations;


namespace Shot.Licensing.Api.Data
{
    public class License
    {
        [Key]
        public int LicenseId { get; set; }

        [Required]
        public Guid LicenseUuid { get; set; }

        [Required]
        public Guid ProductUuid { get; set; }

        [Required]
        public string LicenseString { get; set; }

        public string LicenseAttributes { get; set; }

        [Required]
        public string LicenseChecksum { get; set; }

        public string AttributesChecksum { get; set; }

        [Required]
        public string ChecksumType { get; set; }

        public bool? IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedDateTimeUtc { get; set; }

        [Required]
        public DateTime ModifiedDateTimeUtc { get; set; }

        [Required]
        public string CreatedByUser { get; set; }

        [Required]
        public string ModifiedByUser { get; set; }
    }
}
