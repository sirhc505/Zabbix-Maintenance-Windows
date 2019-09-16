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
    }

    public partial class MaintWinGroup
    {
        [JsonProperty("GroupName")]
        public string GroupName { get; set; }

        [JsonProperty("TemplateID")]
        public long TemplateId { get; set; }
    }
}

