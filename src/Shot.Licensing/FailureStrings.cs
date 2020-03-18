using System;
using System.Linq;
using System.Collections.Generic;
using samplesl.Validation;


namespace samplesl
{
    public static class FailureStrings
    {
        private static IStringLocalizer _localizer = new ResourceStringLocalizer();

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

        public const string ACT07Code       = "ACT07Code";
        public const string ACT07Message    = "ACT07Message";
        public const string ACT07Resolve    = "ACT07Resolve";

        public const string ACT08Code       = "ACT08Code";
        public const string ACT08Message    = "ACT08Message";
        public const string ACT08Resolve    = "ACT08Resolve";

        public const string ACT09Code       = "ACT09Code";
        public const string ACT09Message    = "ACT09Message";
        public const string ACT09Resolve    = "ACT09Resolve";

        public const string ACT10Code       = "ACT10Code";
        public const string ACT10Message    = "ACT10Message";
        public const string ACT10Resolve    = "ACT10Resolve";

        public const string ACT11Code       = "ACT11Code";
        public const string ACT11Message    = "ACT11Message";
        public const string ACT11Resolve    = "ACT11Resolve";

        public const string ACT12Code       = "ACT12Code";
        public const string ACT12Message    = "ACT12Message";
        public const string ACT12Resolve    = "ACT12Resolve";

        public const string ACT13Code       = "ACT13Code";
        public const string ACT13Message    = "ACT13Message";
        public const string ACT13Resolve    = "ACT13Resolve";

        public const string ACT14Code       = "ACT14Code";
        public const string ACT14Message    = "ACT14Message";
        public const string ACT14Resolve    = "ACT14Resolve";

        public const string ACT15Code       = "ACT15Code";
        public const string ACT15Message    = "ACT15Message";
        public const string ACT15Resolve    = "ACT15Resolve";

        public const string ACT16Code       = "ACT16Code";
        public const string ACT16Message    = "ACT16Message";
        public const string ACT16Resolve    = "ACT16Resolve";

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

            // TODO: cache those generated failures

            var c = code.Replace("Code", "");
            var m = $"{c}Message";
            var r = $"{c}Resolve";

            return new GeneralValidationFailure
            {
                Code = _localizer[code],
                Message = _localizer[m],
                HowToResolve = _localizer[r],
            };
        }

        public static void SetLocalizer(IStringLocalizer localizer)
        {
            if (localizer == null)
            {
                throw new ArgumentNullException(nameof(localizer));
            }
            _localizer = localizer;
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

            yield return ACT07Code;
            yield return ACT07Message;
            yield return ACT07Resolve;

            yield return ACT08Code;
            yield return ACT08Message;
            yield return ACT08Resolve;

            yield return ACT09Code;
            yield return ACT09Message;
            yield return ACT09Resolve;

            yield return ACT10Code;
            yield return ACT10Message;
            yield return ACT10Resolve;

            yield return ACT11Code;
            yield return ACT11Message;
            yield return ACT11Resolve;

            yield return ACT12Code;
            yield return ACT12Message;
            yield return ACT12Resolve;

            yield return ACT13Code;
            yield return ACT13Message;
            yield return ACT13Resolve;

            yield return ACT14Code;
            yield return ACT14Message;
            yield return ACT14Resolve;

            yield return ACT15Code;
            yield return ACT15Message;
            yield return ACT15Resolve;

            yield return ACT16Code;
            yield return ACT16Message;
            yield return ACT16Resolve;
        }

        #endregion
    }
}
