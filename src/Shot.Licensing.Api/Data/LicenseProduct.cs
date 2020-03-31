using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Shot.Licensing.Api.Data
{
    public class LicenseProduct
    {
        [Key]
        public int LicenseProductId { get; set; }

        [Required]
        public Guid ProductUuid { get; set; }

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
