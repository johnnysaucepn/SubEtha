using System;
using System.Xml.Serialization;

namespace Howatworks.EliteDangerous.Bindings
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
        public Setting<float> Deadzone { get; set; }
    }
}
