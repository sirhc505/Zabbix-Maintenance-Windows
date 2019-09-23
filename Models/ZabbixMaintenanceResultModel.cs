using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ZabbixMW.Models
{
    public partial class ZabbixMaintenanceResultModel
    {
        [JsonProperty("jsonrpc")]
        public string Jsonrpc { get; set; }

        [JsonProperty("result")]
        public ZabbixMaintenanceResult Result { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }
    }

    public partial class ZabbixMaintenanceResult
    {
        [JsonProperty("maintenanceids")]
        public List<long> Maintenanceids { get; set; }
    }
}
