using System;
using System.ComponentModel.DataAnnotations;


namespace Shot.Licensing.Svr.Data
{
    public class License
    {
        [Key]
        public int LicenseId { get; set; }
        public Guid LicenseUuid { get; set; }
        public string LicenseString { get; set; }
        public string Checksum { get; set; }
        public string ChecksumType { get; set; }

        public bool IsActive { get; set; }
        
        public DateTime CreatedDateTimeUtc { get; set; }
        public DateTime ModifiedDateTimeUtc { get; set; }
        public string CreatedByUser { get; set; }
        public string ModifiedByUser { get; set; }
    }
}
