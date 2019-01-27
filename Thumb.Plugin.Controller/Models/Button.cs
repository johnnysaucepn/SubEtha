using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Thumb.Plugin.Controller.Models
{
    [Serializable]
    public class Button
    {
        [Serializable]
        public class ButtonBinding
        {
            [XmlAttribute]
            public string Device { get; set; }

            [XmlAttribute]
            public string Key { get; set; }

            [XmlElement]
            public List<ButtonModifier> Modifier { get; set; }
        }

        [Serializable]
        public class ButtonModifier
        {
            [XmlAttribute]
            public string Key { get; set; }
        }

        [XmlElement]
        public ButtonBinding Primary { get; set; }

        [XmlElement]
        public ButtonBinding Secondary { get; set; }

        [XmlElement]
        public Setting<bool> ToggleOn { get; set; } // TODO: check data type
    }
}
