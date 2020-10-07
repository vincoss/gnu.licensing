using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


namespace Gnu.Licensing.Core.Entities.Configuration
{
    public class EnumToInt : ValueConverter<LicenseType, int>
    {
        public static readonly EnumToInt Instance = new EnumToInt();

        public EnumToInt()
            : base(
                v => (int)v,
                v => (LicenseType)v)
        {
        }
    }
}
