using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZabbixMW.Models
{
    public partial class ZabbixRpcAuthModel
    {
        [JsonProperty("jsonrpc")]
        public string Jsonrpc { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("params")]
        public Params Params { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("auth")]
        public object Auth { get; set; }
    }

    public partial class Params
    {
        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
