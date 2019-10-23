using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistryPrototype.BE
{
    [JsonObject]
    public class MinimalPackage
    {
        [JsonProperty("versions")]
        public string Versions { get; set; }
        [JsonProperty("dist-tags")]
        public string DistTags { get; set; }
        [JsonProperty("modified")]
        public string Modified { get; set; }
        [JsonProperty("name")]
        public string _ID { get; set; }
        [JsonIgnore]
        public string RawMetaData { get; set; }
    }
}
