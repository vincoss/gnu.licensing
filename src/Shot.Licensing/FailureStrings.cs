//
// Copyright © 2003-2020 https://github.com/vincoss/Shot.Licensing
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

using System;
using System.Linq;
using System.Collections.Generic;
using Shot.Licensing.Validation;
using System.Collections.Concurrent;
using System.Resources;
using System.Reflection;
using System.Globalization;

namespace Shot.Licensing
{
    public static class FailureStrings
    {
        #region Keys

        public const string ACT00Code       = "ACT00Code";
        public const string ACT00Message    = "ACT00Message";
        public const string ACT00Resolve    = "ACT00Resolve";

        public const string ACT01Code       = "ACT01Code";
        public const string ACT01Message    = "ACT01Message";
        public const string ACT01Resolve    = "ACT01Resolve";

        public const string ACT02Code       = "ACT02Code";
        public const string ACT02Message    = "ACT02Message";
        public const string ACT02Resolve    = "ACT02Resolve";

        public const string ACT03Code       = "ACT03Code";
        public const string ACT03Message    = "ACT03Message";
        public const string ACT03Resolve    = "ACT03Resolve";

        public const string ACT04Code       = "ACT04Code";
        public const string ACT04Message    = "ACT04Message";
        public const string ACT04Resolve    = "ACT04Resolve";

        public const string ACT05Code       = "ACT05Code";
        public const string ACT05Message    = "ACT05Message";
        public const string ACT05Resolve    = "ACT05Resolve";

        public const string ACT06Code       = "ACT06Code";
        public const string ACT06Message    = "ACT06Message";
        public const string ACT06Resolve    = "ACT06Resolve";

        public const string VAL00Code       = "VAL00Code";
        public const string VAL00Message    = "VAL00Message";
        public const string VAL00Resolve    = "VAL00Resolve";

        public const string VAL01Code       = "VAL01Code";
        public const string VAL01Message    = "VAL01Message";
        public const string VAL01Resolve    = "VAL01Resolve";

        public const string VAL02Code       = "VAL02Code";
        public const string VAL02Message    = "VAL02Message";
        public const string VAL02Resolve    = "VAL02Resolve";

        public const string VAL03Code       = "VAL03Code";
        public const string VAL03Message    = "VAL03Message";
        public const string VAL03Resolve    = "VAL03Resolve";

        public const string VAL04Code       = "VAL04Code";
        public const string VAL04Message    = "VAL04Message";
        public const string VAL04Resolve    = "VAL04Resolve";

        public const string VAL05Code       = "VAL05Code";
        public const string VAL05Message    = "VAL05Message";
        public const string VAL05Resolve    = "VAL05Resolve";

        #endregion

        #region Public methods

        public static IValidationFailure Get(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException(nameof(code));
            }
            if (GetKeys().Any(x => x.Equals(code, StringComparison.OrdinalIgnoreCase))== false)
            {
                throw new InvalidOperationException($"Code '{code}' not found.");
            }

            var c = code.Replace("Code", "");
            var m = $"{c}Message";
            var r = $"{c}Resolve";

            return new GeneralValidationFailure
            {
                Code = Translate(code),
                Message = Translate(m),
                HowToResolve = Translate(r),
            };
        }

        public static IEnumerable<string> GetKeys()
        {
            yield return ACT00Code;
            yield return ACT00Message;
            yield return ACT00Resolve;

            yield return ACT01Code;
            yield return ACT01Message;
            yield return ACT01Resolve;

            yield return ACT02Code;
            yield return ACT02Message;
            yield return ACT02Resolve;

            yield return ACT03Code;
            yield return ACT03Message;
            yield return ACT03Resolve;

            yield return ACT04Code;
            yield return ACT04Message;
            yield return ACT04Resolve;

            yield return ACT05Code;
            yield return ACT05Message;
            yield return ACT05Resolve;

            yield return ACT06Code;
            yield return ACT06Message;
            yield return ACT06Resolve;

            yield return VAL00Code;
            yield return VAL00Message;
            yield return VAL00Resolve;

            yield return VAL01Code;
            yield return VAL01Message;
            yield return VAL01Resolve;

            yield return VAL02Code;
            yield return VAL02Message;
            yield return VAL02Resolve;

            yield return VAL03Code;
            yield return VAL03Message;
            yield return VAL03Resolve;

            yield return VAL04Code;
            yield return VAL04Message;
            yield return VAL04Resolve;

            yield return VAL05Code;
            yield return VAL05Message;
            yield return VAL05Resolve;
        }

        #endregion

        private static readonly string ResourceId = "Shot.Licensing.Resources.AppResources";
        static readonly Lazy<ResourceManager> ResMgr = new Lazy<ResourceManager>(
        () => new ResourceManager(ResourceId, IntrospectionExtensions.GetTypeInfo(typeof(FailureStrings)).Assembly));

        private static string Translate(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var translation = ResMgr.Value.GetString(key);
            if (translation == null)
            {
                throw new ArgumentException($"Key '{key}' was not found in resources '{ResourceId}' for culture '{CultureInfo.CurrentCulture}'.");
            }
            return translation;
        }
    }
}
