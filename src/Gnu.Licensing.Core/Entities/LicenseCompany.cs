using System;
using System.ComponentModel.DataAnnotations;


namespace Gnu.Licensing.Core.Entities
{
    public class LicenseCompany
    {
        [Key]
        public Guid LicenseCompanyId { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public DateTime CreatedDateTimeUtc { get; set; }

        [Required]
        public string CreatedByUser { get; set; }
    }
}
