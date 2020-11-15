using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;
using static Howatworks.SubEtha.Journal.Exploration.Scan;

namespace Howatworks.SubEtha.Parser.Test
{
    public class ParentItemConverterTests
    {
        private class SampleClass
        {
            public string Name { get; set; }
            public int Number { get; set; }
            public List<ParentItem> Parents { get; set; }
        }

        private static string JsonSample = @"{""Name"":""Sol"",""Number"":1,""Parents"":[{""Star"":1},{""Planet"":2}]}";
        private static SampleClass ObjectSample = new SampleClass
        {
            Name = "Sol",
            Number = 1,
            Parents = new List<ParentItem> {
                new ParentItem { BodyType = "Star", BodyID = 1 },
                new ParentItem { BodyType = "Planet", BodyID = 2 }
            }
        };

        [Fact]
        public void ReadParentItem()
        {
            var converter = new ParentItemConverter();

            var obj = JsonConvert.DeserializeObject<SampleClass>(JsonSample, converter);

            Assert.Equal(ObjectSample.Name, obj.Name);
            Assert.Equal(ObjectSample.Number, obj.Number);
            Assert.Equal(ObjectSample.Parents.Count, obj.Parents.Count);
            Assert.Equal(ObjectSample.Parents[1].BodyType, obj.Parents[1].BodyType);
            Assert.Equal(ObjectSample.Parents[0].BodyID, obj.Parents[0].BodyID);
        }

        [Fact]
        public void WriteParentItem()
        {
            var converter = new ParentItemConverter();

            string json = JsonConvert.SerializeObject(ObjectSample, converter);

            Assert.Equal(JsonSample, json);
        }
    }
}
