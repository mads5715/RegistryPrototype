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
        public int Total
        {
            get
            {
                if (Packages != null)
                {
                    return Packages.Count;
                }
                else { return 0; }
            }
        }

        [JsonProperty("time")]
        public DateTime Date { get { return DateTime.Now; } }
    }
}
