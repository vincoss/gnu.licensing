using System.ComponentModel.DataAnnotations;


namespace Gnu.Licensing.Api.ViewModels
{
    public class LicenseProductViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string SignKeyName { get; set; }
    }
}
