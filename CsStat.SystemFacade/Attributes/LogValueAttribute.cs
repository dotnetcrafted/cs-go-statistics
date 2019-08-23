using System;

namespace CsStat.SystemFacade.Attributes
{
    public class LogValueAttribute : Attribute
    {
        public LogValueAttribute(string value) => Value = value;
        public string Value { get; private set; }
    }
}