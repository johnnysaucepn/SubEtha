using System;
using System.Xml.Serialization;

namespace Howatworks.SubEtha.Bindings
{
    [Serializable]
    public class Axis
    {
        [Serializable]
        public class AxisBinding
        {
            [XmlAttribute]
            public string Key { get; set; }
        }

        public AxisBinding Binding { get; set; }
        public Setting<bool> Inverted { get; set; }
        public Setting<decimal> Deadzone { get; set; }
    }
}
