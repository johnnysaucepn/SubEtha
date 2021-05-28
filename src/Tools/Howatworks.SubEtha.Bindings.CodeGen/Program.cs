using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Howatworks.SubEtha.Bindings.CodeGen
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            var xmlFilePath = config["src"];
            var csFilePath = config["dest"] ?? Path.Combine(Path.GetDirectoryName(xmlFilePath), "BindingSet.generated.cs");

            var dedupeList = new List<string>();

            var xmlReader = XDocument.Load(xmlFilePath);
            using (var csWriter = new StreamWriter(csFilePath, false, Encoding.UTF8))
            {
                csWriter.Write(@"namespace Howatworks.SubEtha.Bindings
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(""ReSharper"", ""InconsistentNaming"")]
    public partial class BindingSet
    {
");

                foreach (var item in xmlReader.Root.Elements())
                {
                    if (dedupeList.Contains(item.Name.ToString()))
                    {
                        continue;
                    }

                    var controlName = item.Name;
                    var controlType = "object";
                    var controlComment = "";

                    if (item.Name == "KeyboardLayout")
                    {
                        continue;
                    }

                    var valueAttr = item.Attributes().FirstOrDefault(x => x.Name == "Value");
                    if (valueAttr != null)
                    {
                        if (int.TryParse(valueAttr.Value, out _))
                        {
                            // Int values are probably bools
                            controlType = "Setting<bool>";
                            controlComment = "TODO: check data type";
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
                        var children = item.Elements().ToList();
                        if (children.Any(x => x.Name == "Primary") && children.Any(x => x.Name == "Secondary"))
                        {
                            controlType = "Button";

                            if (children.Any(x => x.Name == "ToggleOn"))
                            {
                                controlType = "ToggleButton";
                            }
                        }
                        if (children.Any(x => x.Name == "Binding"))
                        {
                            controlType = "Axis";
                        }
                    }

                    csWriter.Write($"       public {controlType} {controlName} {{ get; set; }}");
                    csWriter.WriteLine(string.IsNullOrWhiteSpace(controlComment) ? string.Empty : $" // {controlComment}");

                    dedupeList.Add(controlName.ToString());
                }
                csWriter.Write(@"   }
}
");
            }
        }
    }
}
