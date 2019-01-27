using System;
using System.Xml.Serialization;

namespace Thumb.Plugin.Controller.Models
{
    [Serializable]
    public class Setting<T>
    {
        [XmlAttribute]
        public T Value { get; set; }
    }
}
