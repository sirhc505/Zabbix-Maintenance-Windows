using Newtonsoft.Json;

namespace ZabbixMW.Models
{
    public partial class ZabbixRpcAuthModel
    {
        [JsonProperty("jsonrpc")]
        public string Jsonrpc { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("params")]
        public ZabbixRpcAuthParams Params { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("auth")]
        public string Auth { get; set; }

        [JsonProperty("result")]
        public string Result { get; set; }
    }

    public partial class ZabbixRpcAuthParams
    {
        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
