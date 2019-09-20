using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZabbixMW.Models
{
    public class ADMaintWindowModel
    {
        [JsonProperty("ADOU")]
        public string Adou { get; set; }

        [JsonProperty("MaintWinGroups")]
        public MaintWinGroup[] MaintWinGroups { get; set; }

        [JsonProperty("Default_TemplateID")]
        public long DefaultTemplateId { get; set; }

        [JsonProperty("Zabbix_UserName")]
        public string ZabbixUserName { get; set; }

        [JsonProperty("Zabbix_Password")]
        public string ZabbixPassword { get; set; }

        [JsonProperty("Zabbix_ServerName")]
        public string ZabbixServerName { get; set; }

        [JsonProperty("ADSettings")]
        public AdSettings AdSettings { get; set; }
    }

    public partial class MaintWinGroup
    {
        [JsonProperty("GroupName")]
        public string GroupName { get; set; }

        [JsonProperty("TemplateID")]
        public int TemplateId { get; set; }
    }

    public partial class AdSettings
    {
        [JsonProperty("ADOU")]
        public string Adou { get; set; }

        [JsonProperty("ADServer")]
        public string AdServer { get; set; }

        [JsonProperty("ADUser")]
        public string AdUser { get; set; }

        [JsonProperty("ADPassword")]
        public string AdPassword { get; set; }
    }

    public partial class ZabbixSettings
    {
        [JsonProperty("Zabbix_UserName")]
        public string ZabbixUserName { get; set; }

        [JsonProperty("Zabbix_Password")]
        public string ZabbixPassword { get; set; }

        [JsonProperty("Zabbix_ServerName")]
        public string ZabbixServerName { get; set; }

        [JsonProperty("Zabbix_DefaultID")]
        public int ZabbixDefaultID { get; set; }

        [JsonProperty("Zabbix_JSON_RPC")]
        public string ZabbixJsonRpc { get; set; }

    }

}

