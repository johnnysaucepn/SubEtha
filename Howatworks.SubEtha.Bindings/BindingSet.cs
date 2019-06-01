using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace Howatworks.SubEtha.Bindings
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public partial class BindingSet
    {
        [XmlAttribute]
        public string PresetName { get; set; }
        [XmlAttribute]
        public int MajorVersion { get; set; }
        [XmlAttribute]
        public int MinorVersion { get; set; }
        [XmlAttribute]
        public int SortOrder { get; set; }

        public string KeyboardLayout { get; set; }
    }
}
