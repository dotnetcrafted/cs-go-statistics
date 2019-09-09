using System;

namespace CsStat.SystemFacade.Attributes
{
    public class Caption : Attribute
    {
        public Caption(string value) => Value = value;
        public string Value { get; private set; }
    }
}