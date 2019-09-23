using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ZabbixMW.Models
{
    public partial class ZabbixSetHostMaintModel
    {
        [JsonProperty("jsonrpc")]
        public string Jsonrpc { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("params")]
        public ZabbixSetHostMaintParams Params { get; set; }

        [JsonProperty("auth")]
        public string Auth { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }
    }

    public partial class ZabbixSetHostMaintParams
    {
        [JsonProperty("maintenanceid")]
        public string Maintenanceid { get; set; }

        [JsonProperty("hostids")]
        public List<string> Hostids { get; set; }
    }
}
