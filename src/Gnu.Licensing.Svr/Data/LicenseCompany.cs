using System;
using System.ComponentModel.DataAnnotations;


namespace Gnu.Licensing.Svr.Data
{
    public class LicenseCompany
    {
        [Key]
        public int LicenseCompanyId { get; set; }

        [Required]
        public Guid CompanyUuid { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public DateTime CreatedDateTimeUtc { get; set; }

        [Required]
        public string CreatedByUser { get; set; }
    }
}
