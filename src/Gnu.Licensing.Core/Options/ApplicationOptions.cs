using System;


namespace Gnu.Licensing.Core.Options
{
    public class ApplicationOptions
    {
        /// <summary>
        /// The URLs the server will use.
        /// </summary>
        public string Urls { get; set; }

        public bool RunMigrationsAtStartup { get; set; } = false;

        public bool UseCustomizationData { get; set; } = false;

        public string PathBase { get; set; }

        public DatabaseOptions Database { get; set; }
    }
}
