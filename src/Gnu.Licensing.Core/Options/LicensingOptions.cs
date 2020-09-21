using System;
using System.Collections.Generic;
using System.Text;


namespace Gnu.Licensing.Core.Options
{
    public class LicensingOptions
    {
        /// <summary>
        /// The URLs the server will use.
        /// </summary>
        public string Urls { get; set; }

        /// <summary>
        /// TODO: Not for the startup
        /// </summary>
        public bool RunMigrationsAtStartup { get; set; } = true;

        public string PathBase { get; set; }

        public DatabaseOptions Database { get; set; }
    }
}
