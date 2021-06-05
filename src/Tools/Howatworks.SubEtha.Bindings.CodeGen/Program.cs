using Howatworks.SubEtha.Bindings.Monitor;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Howatworks.SubEtha.Bindings.CodeGen
{
    internal static class Program
    {
        private static readonly JsonSerializerSettings _serializer = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            Converters = { new StringEnumConverter() }
        };

        private static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            var xmlFilePath = config["src"];
            var defaultOutputPath = Path.GetDirectoryName(xmlFilePath);
            var catJsonFilePath = config["cat"] ?? Path.Combine(defaultOutputPath, "BindingCategories.json");
            var setCsFilePath = config["set"] ?? Path.Combine(defaultOutputPath, "BindingSet.generated.cs");
            var lookupCsFilePath = config["lookup"] ?? Path.Combine(defaultOutputPath, "BindingLookup.generated.cs");

            var dedupeList = new List<string>();
            UpdateSet(xmlFilePath, setCsFilePath, dedupeList);
            UpdateLookup(catJsonFilePath, lookupCsFilePath);
        }

        private static void UpdateSet(string xmlFilePath, string setCsFilePath, List<string> dedupeList)
        {
            var xmlReader = XDocument.Load(xmlFilePath);
            using (var csWriter = new StreamWriter(setCsFilePath, false, Encoding.UTF8))
            {
                var ns = typeof(BindingSet).Namespace;
                csWriter.Write($@"namespace {ns}
{{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(""ReSharper"", ""InconsistentNaming"")]
    public partial class BindingSet
    {{
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

        private static void UpdateLookup(string catJsonFilePath, string lookupCsFilePath)
        {
            Dictionary<string, BindingCategory> catLookup = ReadCategoryLookup(catJsonFilePath);

            using (var csWriter = new StreamWriter(lookupCsFilePath, false, Encoding.UTF8))
            {
                var ns = typeof(BindingLookup).Namespace;

                csWriter.Write($@"using System.Collections.Generic;

namespace {ns}
{{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(""ReSharper"", ""InconsistentNaming"")]
    public partial class BindingLookup
    {{
        public Dictionary<string, BindingCategory> Lookup = new Dictionary<string, BindingCategory> {{
");

                foreach (var controlName in catLookup.Keys)
                {
                    BindingCategory controlCategory = BindingCategory.Unknown;
                    if (catLookup.ContainsKey(controlName))
                    {
                        controlCategory = catLookup[controlName];
                    }
                    else
                    {
                        catLookup[controlName] = BindingCategory.Unknown;
                    }

                    csWriter.WriteLine($"            [nameof(BindingSet.{controlName})] = BindingCategory.{controlCategory},");
                }


                csWriter.Write(@"        };
    }
}
");
            }

            WriteCategoryLookup(catJsonFilePath, catLookup);
        }

        private static Dictionary<string, BindingCategory> ReadCategoryLookup(string catJsonFilePath)
        {
            var emptySet = new Dictionary<string, BindingCategory>();
            try
            {
                var catJson = File.ReadAllText(catJsonFilePath);
                return JsonConvert.DeserializeObject<Dictionary<string, BindingCategory>>(catJson) ?? emptySet;
            }
            catch (Exception ex) when (ex is JsonSerializationException || ex is IOException)
            {
                Console.Error.WriteLine($"Could not read category file '{catJsonFilePath}' - {ex}");
                return emptySet;
            }
        }

        private static void WriteCategoryLookup(string catJsonFilePath, Dictionary<string, BindingCategory> categories)
        {
            try
            {
                var catJson = JsonConvert.SerializeObject(categories, _serializer);
                File.WriteAllText(catJsonFilePath, catJson);
            }
            catch (Exception ex) when (ex is JsonSerializationException || ex is IOException)
            {
                Console.Error.WriteLine($"Could not update category file '{catJsonFilePath}' - {ex}");
            }
        }
    }
}
