using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistryPrototype.BE
{
    public class LoginResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("ok")]
        public bool IsOk { get; set; }
    }
}
