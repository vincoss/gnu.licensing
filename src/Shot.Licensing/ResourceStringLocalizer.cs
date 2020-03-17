using samplesl.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;

namespace samplesl
{
    public class ResourceStringLocalizer : IStringLocalizer
    {
        private static readonly string ResourceId = typeof(AppResources).FullName;
        static readonly Lazy<ResourceManager> ResMgr = new Lazy<ResourceManager>(
        () => new ResourceManager(ResourceId, IntrospectionExtensions.GetTypeInfo(typeof(ResourceStringLocalizer)).Assembly));

        private string Get(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var translation = ResMgr.Value.GetString(key);
            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException($"Key '{key}' was not found in resources '{ResourceId}' for culture '{CultureInfo.CurrentCulture}'.");
#else
                translation = key; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
            }
            return translation;
        }

        public string this[string name]
        {
            get { return Get(name); }
        }

    }
}
