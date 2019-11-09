using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistryPrototype.BE
{
    [JsonObject]
    public class MinimalPackage
    {
        private DateTime modified;
        private string _rawMetaData;
        [JsonIgnore]
        public string DistTags;
        [JsonIgnore]
        public string Versions { get; set; }
        [JsonProperty("versions")]
        public JObject PublicVersions { get { return JObject.Parse(Versions); } set { Versions = value.ToString(); } }
        [JsonProperty("dist-tags")]
        public JObject PublicDistTags { get { return JObject.Parse(DistTags); } set { DistTags = value.ToString(); } }
        [JsonProperty("modified")]
        public string Modified { get { return modified.ToString("yyyy-MM-ddTHH:mm:ssZ"); } set { modified = DateTime.Parse(value); } }
        [JsonProperty("name")]
        public string _ID { get; set; }
        [JsonIgnore]
        public string RawMetaData { get { return _rawMetaData; } set { _rawMetaData = value;
                var jsonObj = JObject.Parse(value);
                Versions = jsonObj["versions"].ToString();
                DistTags = jsonObj["dist-tags"].ToString();
            } }
    }
}
