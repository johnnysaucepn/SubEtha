using System;
using System.Xml.Serialization;

namespace Thumb.Plugin.Controller.Models
{
    [Serializable]
    public class Binding
    {

        [XmlAttribute]
        public string PresetName { get; set; }
        [XmlAttribute]
        public int MajorVersion { get; set; }
        [XmlAttribute]
        public int MinorVersion { get; set; }

        [XmlElement]
        public string KeyboardLayout { get; set; }

        [XmlElement]
        public Axis YawAxisRaw { get; set; }

        [XmlElement]
        public Button YawLeftButton { get; set; }

        [XmlElement]
        public Button LeftThrustButton { get; set; }
    }
}
