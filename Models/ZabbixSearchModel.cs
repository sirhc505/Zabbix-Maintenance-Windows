using Newtonsoft.Json;
using System.Collections.Generic;

namespace ZabbixMW.Models
{
    public partial class ZabbixSearchModel
    {
        [JsonProperty("jsonrpc")]
        public string Jsonrpc { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("params")]
        public ZabbixSearcParams Params { get; set; }

        [JsonProperty("auth")]
        public string Auth { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }
    }

    public partial class ZabbixSearcParams
    {
        [JsonProperty("search")]
        public ZabbixSearcFilter Filter { get; set; }
    }

    public partial class ZabbixSearcFilter
    {
        [JsonProperty("host")]
        public List<string> Host { get; set; }
    }
}