using System;
using System.Collections.Generic;
using System.Text;

namespace samplesl.Sample_XamarinForms
{
    public interface IApplicationContext
    {
        string GetValueOrDefault(string key, string defaultValue);
    }
}
