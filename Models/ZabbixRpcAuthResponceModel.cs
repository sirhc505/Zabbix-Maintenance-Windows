﻿using Newtonsoft.Json;

namespace ZabbixMW.Models
{
    public partial class ZabbixRpcAuthResponceModel
    {
        [JsonProperty("jsonrpc")]
        public string Jsonrpc { get; set; }

        [JsonProperty("result")]
        public string Result { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
