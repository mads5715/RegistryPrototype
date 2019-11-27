using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistryPrototype.BE
{
    public class SearchObject
    {
        [JsonProperty("objects")]
        public List<SearchPackage> Packages { get; set; }

        [JsonProperty("total")]
        public int Total { get { return Packages.Count; } }

        [JsonProperty("time")]
        public DateTime Date { get { return DateTime.Now; } }
    }
}
