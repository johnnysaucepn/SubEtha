using System;
using System.Xml.Serialization;

namespace Thumb.Plugin.Controller.Models
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

        [XmlElement]
        public AxisBinding Binding { get; set; }
    }
}
