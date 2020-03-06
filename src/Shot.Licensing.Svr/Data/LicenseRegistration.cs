using System;
using System.ComponentModel.DataAnnotations;


namespace samplesl.Svr.Data
{
    public class LicenseRegistration
    {
        [Key]
        public int LicenseRegistrationId { get; set; }
        public Guid LicenseUuid { get; set; }

        public string ProductName { get; set; } // TODO: Guid
        public string LicenseName { get; set; }
        public string LicenseEmail { get; set; }
        public bool IsActive { get; set; } // Blacklist
        public DateTime CreatedDateTimeUtc { get; set; }
        public string CreatedByUser { get; set; }
    }
}
