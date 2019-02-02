using System;
using System.Xml.Serialization;

namespace Howatworks.EliteDangerous.Bindings
{
    [Serializable]
    public class Setting<T>
    {
        [XmlAttribute]
        public T Value { get; set; }
    }
}
