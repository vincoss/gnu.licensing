using samplesl.Validation;
using System;


namespace samplesl
{
    public class Errors
    {
        private static IStringLocalizer _localizer = new ResourceStringLocalizer();


        #region Public

        public const string ACT00Code = "ACT00Code";
        public const string ACT00Message = "ACT00Message";
        public const string ACT00Resolve = "ACT00Resolve";

        public IValidationFailure Get(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            var localKey = key.ToUpper();
            switch (localKey)
            {
                case ACT00Code:
                    return ACT00_Message;
                default:
                    {
                        throw new InvalidOperationException($"Invalid '{localKey}' key.");
                    }
            }
        }

        public static void SetLocalizer(IStringLocalizer localizer)
        {
            if (localizer == null)
            {
                throw new ArgumentNullException(nameof(localizer));
            }
            _localizer = localizer;
        }

        #endregion

        #region Failures

        private static IValidationFailure ACT00_Message = new GeneralValidationFailure
        {
            Code = _localizer[ACT00Code],
            Message = _localizer[ACT00Message],
            HowToResolve = _localizer[ACT00Resolve],
        };

        #endregion

    }
}
