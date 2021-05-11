using System;
using System.Xml.Serialization;

namespace Howatworks.SubEtha.Bindings
{
    [Serializable]
    public class Setting<T>
    {
        [XmlAttribute]
        public T Value { get; set; }
    }
}
