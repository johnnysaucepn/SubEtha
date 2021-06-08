using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Howatworks.SubEtha.Bindings
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
            public List<ButtonModifier> Modifier { get; set; } = new List<ButtonModifier>();

            [XmlElement]
            public ButtonHold Hold { get; set; } = new ButtonHold();
        }

        [Serializable]
        public class ButtonModifier
        {
            [XmlAttribute]
            public string Device { get; set; }

            [XmlAttribute]
            public string Key { get; set; }
        }

        [Serializable]
        public class ButtonHold
        {
            [XmlAttribute]
            public bool Value { get; set; }

        }

        [XmlElement]
        public ButtonBinding Primary { get; set; }

        [XmlElement]
        public ButtonBinding Secondary { get; set; }
    }

    public class ToggleButton : Button
    {
        [XmlElement]
        public Setting<bool> ToggleOn { get; set; } // TODO: check data type
    }
}
