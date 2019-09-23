using Newtonsoft.Json;

namespace ZabbixMW.Models
{
    public partial class SearchMaintenanceIdModel
    {
        [JsonProperty("jsonrpc")]
        public string Jsonrpc { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("params")]
        public SearchMaintenanceIdParams Params { get; set; }

        [JsonProperty("auth")]
        public string Auth { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }
    }

    public partial class SearchMaintenanceIdParams
    {
        [JsonProperty("maintenanceid")]
        public string Maintenanceid { get; set; }
    }
}
