using System;
using System.Collections.Generic;
using System.Text;


namespace samplesl
{
    public interface IStringLocalizer
    {
        string this[string name] { get; }
    }
}
