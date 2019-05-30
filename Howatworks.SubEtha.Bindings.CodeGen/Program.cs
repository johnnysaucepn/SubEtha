using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Howatworks.SubEtha.Bindings.CodeGen
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            var xmlFilePath = config["src"];
            var csFilePath = config["dest"];

            var xmlReader = XDocument.Load(xmlFilePath);
            using (var csWriter = new StreamWriter(csFilePath, false, Encoding.UTF8))
            {
                csWriter.Write(@"
using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace Howatworks.SubEtha.Bindings
{
    [Serializable]
    [SuppressMessage(""ReSharper"", ""InconsistentNaming"")]
    public class GeneratedBindingSet
        {

            [XmlAttribute]
            public string PresetName { get; set; }
            [XmlAttribute]
            public int MajorVersion { get; set; }
            [XmlAttribute]
            public int MinorVersion { get; set; }
            [XmlAttribute]
            public int SortOrder { get; set; }
            ");

                foreach (var item in xmlReader.Root.Elements())
                {
                    var controlType = "object";
                    var controlName = item.Name;
                    var controlComment = "";

                    var valueAttr = item.Attributes().FirstOrDefault(x => x.Name == "Value");
                    if (valueAttr != null)
                    {
                        if (string.IsNullOrWhiteSpace(valueAttr.Value))
                        {
                            controlType = "Setting<string>";
                            controlComment = "TODO: check type";
                        }
                        else if (int.TryParse(valueAttr.Value, out _))
                        {
                            // Int values are probably bools
                            controlType = "Setting<bool>";
                            controlComment = "TODO: check type";
                        }
                        else if (decimal.TryParse(valueAttr.Value, out _))
                        {
                            controlType = "Setting<decimal>";
                        }
                        
                        else
                        {
                            controlType = "Setting<string>";
                            controlComment = "TODO: enum?";
                        }
                    }
                    else
                    {
                        var children = item.Elements();
                        if (children.Any(x => x.Name == "Primary") && children.Any(x => x.Name == "Secondary"))
                        {
                            controlType = "Button";
                        }
                        if (children.Any(x => x.Name == "Binding"))
                        {
                            controlType = "Axis";
                        }
                    }

                    if (string.IsNullOrWhiteSpace(controlComment))
                    {
                        csWriter.WriteLine($"public {controlType} {controlName} {{ get; set; }}");
                    }
                    else
                    {
                        csWriter.WriteLine($"public {controlType} {controlName} {{ get; set; }} // {controlComment}");
                    }
                }
                csWriter.Write(@"}
}
");

            }
        }
    }
}
