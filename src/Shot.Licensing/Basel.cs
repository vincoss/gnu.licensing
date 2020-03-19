using samplesl.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace samplesl
{
    public abstract class Basel
    {
        public Task<LicenseResult> ValidateAsync()
        {
            var task = Task.Run(() =>
            {
                var results = new List<IValidationFailure>();
                License actual = null;

                try
                {
                    using (var stream = LicenseOpenRead())
                    {
                        if (stream == null)
                        {
                            var nf = FailureStrings.Get(FailureStrings.ACT08Code);
                            return new LicenseResult(null, null, new[] { nf });
                        }

                        actual = License.Load(stream);
                    }

                    var failures = ValidateInternal(actual);

                    foreach (var f in failures)
                    {
                        results.Add(f);
                    }

                    return new LicenseResult(results.Any() ? null : actual, null, results);
                }
                catch (Exception ex)
                {
                    // TODO: log

                    var failure = FailureStrings.Get(FailureStrings.ACT09Code);

                    results.Add(failure);
                    return new LicenseResult(null, ex, results);
                }
            });
            return task;
        }

        protected abstract IEnumerable<IValidationFailure> ValidateInternal(License actual);

        protected abstract Stream LicenseOpenRead();
    }
}
