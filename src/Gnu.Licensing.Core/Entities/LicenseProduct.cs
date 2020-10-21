using System;
using System.ComponentModel.DataAnnotations;


namespace Gnu.Licensing.Core.Entities
{
    public class LicenseProduct
    {
        [Key]
        public Guid LicenseProductId { get; set; }

        [Required]
        public Guid CompanyId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string ProductDescription { get; set; }

        [Required]
        public string SignKeyName { get; set; }

        [Required]
        public DateTime CreatedDateTimeUtc { get; set; }

        [Required]
        public string CreatedByUser { get; set; }
    }
}
