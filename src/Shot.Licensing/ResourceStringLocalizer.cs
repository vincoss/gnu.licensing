//
// Copyright © 2003-2020 https://github.com/vincoss/samplesl
//
// Author:
//  Ferdinand Lukasak
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


using samplesl.Resources;
using System;
using System.Globalization;
using System.Reflection;
using System.Resources;

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
