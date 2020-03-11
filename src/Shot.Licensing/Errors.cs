using samplesl.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace samplesl
{
    public class Errors
    {
        public const string ACT1 = "ACT.1";
        public const string ACT2 = "ACT.2";

        public IValidationFailure Get(string key)
        {
            if(string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            var localKey = key.ToUpper();
            switch (localKey)
            {
                case ACT1:
                    {
                        return new GeneralValidationFailure
                        {
                            Code = localKey,
                            Message = $"Activation failed.",
                            HowToResolve = "Your product is having trouble connecting with License servers."
                        };
                    }

                default:
                    {
                        throw new InvalidOperationException($"Invalid '{localKey}' key.");
                    }
            }
        }
    }
}
