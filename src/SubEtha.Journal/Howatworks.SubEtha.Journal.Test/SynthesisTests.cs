using Howatworks.SubEtha.Journal.Other;
using Newtonsoft.Json;
using Xunit;

namespace Howatworks.SubEtha.Journal.Test
{
    public class SynthesisTests
    {
        private const string Sample = @"{ ""timestamp"":""2018-06-20T23:50:07Z"", ""event"":""Synthesis"", ""Name"":""Repair Basic"", ""Materials"":[ { ""Name"":""iron"", ""Count"":2 }, { ""Name"":""nickel"", ""Count"":1 } ] }";

        [Fact]
        public void Name()
        {
            var synthesis = JsonConvert.DeserializeObject<Synthesis>(Sample);

            Assert.Equal("Repair Basic", synthesis.Name);

        }

        [Fact]
        public void Materials()
        {
            var synthesis = JsonConvert.DeserializeObject<Synthesis>(Sample);

            Assert.Equal(2, synthesis.Materials.Count);
            Assert.Equal("nickel", synthesis.Materials[1].Name);
            Assert.Equal(1, synthesis.Materials[1].Count);
        }
    }
}
