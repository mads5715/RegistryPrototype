using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistryPrototype.BE
{
    public class SearchPackage
    {
        private DateTime modified;
        private string _rawMetaData;

        [JsonIgnore]
        public string maintainers;

        [JsonIgnore]
        public string description { get; set; }

        [JsonProperty("description")]
        public string PublicVersions { get { return description; } set { description = value.ToString(); } }

        [JsonProperty("maintainers")]
        public JArray Maintainers { get { return JArray.Parse(maintainers); } set { maintainers = value.ToString(); } }

        [JsonProperty("date")]
        public string Modified { get { return modified.ToString("yyyy-MM-ddTHH:mm:ssZ"); } set { modified = DateTime.Parse(value); } }

        [JsonProperty("name")]
        public string _ID { get; set; }

        [JsonProperty("version")]
        public string LatestVersion { get; set; }

        [JsonProperty("keywords")]
        public List<string> Keywords { get; set; }

        [JsonIgnore]
        public string RawMetaData
        {
            get { return _rawMetaData; }
            set
            {
                _rawMetaData = value;
                var jsonObj = JObject.Parse(value);
                maintainers = jsonObj["maintainers"].ToString();
                _ID = jsonObj["name"].ToString();
                description = jsonObj["description"].ToString();
                LatestVersion = jsonObj["dist-tags"]["latest"].ToString();
                Keywords = jsonObj["versions"][LatestVersion].ToString().Split(',').ToList();
            }
        }
    }
}
