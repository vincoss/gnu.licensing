using System;
using System.ComponentModel.DataAnnotations;


namespace Shot.Licensing.Svr.Data
{
    public class License
    {
        [Key]
        public int LicenseId { get; set; }

        [Required]
        public int LicenseRegistrationId { get; set; }

        [Required]
        public Guid LicenseUuid { get; set; }

        [Required]
        public Guid ProductUuid { get; set; }

        [Required]
        public string LicenseString { get; set; }

        [Required]
        public string Checksum { get; set; }

        [Required]
        public string ChecksumType { get; set; }

        public bool? IsActive { get; set; }

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
