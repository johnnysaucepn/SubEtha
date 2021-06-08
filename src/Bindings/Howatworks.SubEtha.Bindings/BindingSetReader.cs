using System.IO;
using System.Xml.Serialization;

namespace Howatworks.SubEtha.Bindings
{
    public class BindingSetReader
    {
        private static readonly XmlSerializer _serializer = new XmlSerializer(typeof(BindingSet), new XmlRootAttribute("Root"));

        public FileInfo File { get; }

        public BindingSetReader(FileInfo file)
        {
            File = file;
        }

        public BindingSet Read()
        {
            using (var file = new FileStream(File.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                return (BindingSet)_serializer.Deserialize(file);
            }
        }
    }
}
