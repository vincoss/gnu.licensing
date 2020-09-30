using System;
using System.ComponentModel.DataAnnotations;


namespace Gnu.Licensing.Core.Options
{
    public class DatabaseOptions
    {
        public const string Sqlite = "Sqlite";
        public const string SqlServer = "SqlServer";

        [Required]
        public string Type { get; set; }

        [Required]
        public string ConnectionString { get; set; }
    }
}
