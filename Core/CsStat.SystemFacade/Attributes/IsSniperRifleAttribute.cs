using System;

namespace CsStat.SystemFacade.Attributes
{
    public class IsSniperRifleAttribute : Attribute
    {
        public IsSniperRifleAttribute(bool value) => Value = value;
        public bool Value { get; private set; }
    }
}