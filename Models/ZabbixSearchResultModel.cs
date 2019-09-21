using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ZabbixMW.Models
{
    public partial class ZabbixSearchResultModel
    {
        [JsonProperty("jsonrpc")]
        public string Jsonrpc { get; set; }

        [JsonProperty("result")]
        public List<ZabbixSearchResults> Result { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }

    public partial class ZabbixSearchResults
    {
        [JsonProperty("maintenances")]
        public object[] Maintenances { get; set; }

        [JsonProperty("hostid")]
        public int Hostid { get; set; }

        [JsonProperty("proxy_hostid")]
        public int ProxyHostid { get; set; }

        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("disable_until")]
        public int DisableUntil { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("available")]
        public int Available { get; set; }

        [JsonProperty("errors_from")]
        public int ErrorsFrom { get; set; }

        [JsonProperty("lastaccess")]
        public int Lastaccess { get; set; }

        [JsonProperty("ipmi_authtype")]
        public int IpmiAuthtype { get; set; }

        [JsonProperty("ipmi_privilege")]
        public int IpmiPrivilege { get; set; }

        [JsonProperty("ipmi_username")]
        public string IpmiUsername { get; set; }

        [JsonProperty("ipmi_password")]
        public string IpmiPassword { get; set; }

        [JsonProperty("ipmi_disable_until")]
        public int IpmiDisableUntil { get; set; }

        [JsonProperty("ipmi_available")]
        public int IpmiAvailable { get; set; }

        [JsonProperty("snmp_disable_until")]
        public int SnmpDisableUntil { get; set; }

        [JsonProperty("snmp_available")]
        public int SnmpAvailable { get; set; }

        [JsonProperty("maintenanceid")]
        public int Maintenanceid { get; set; }

        [JsonProperty("maintenance_status")]
        public int MaintenanceStatus { get; set; }

        [JsonProperty("maintenance_type")]
        public int MaintenanceType { get; set; }

        [JsonProperty("maintenance_from")]
        public int MaintenanceFrom { get; set; }

        [JsonProperty("ipmi_errors_from")]
        public int IpmiErrorsFrom { get; set; }

        [JsonProperty("snmp_errors_from")]
        public int SnmpErrorsFrom { get; set; }

        [JsonProperty("ipmi_error")]
        public string IpmiError { get; set; }

        [JsonProperty("snmp_error")]
        public string SnmpError { get; set; }

        [JsonProperty("jmx_disable_until")]
        public int JmxDisableUntil { get; set; }

        [JsonProperty("jmx_available")]
        public int JmxAvailable { get; set; }

        [JsonProperty("jmx_errors_from")]
        public int JmxErrorsFrom { get; set; }

        [JsonProperty("jmx_error")]
        public string JmxError { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("tls_connect")]
        public int TlsConnect { get; set; }

        [JsonProperty("tls_accept")]
        public int TlsAccept { get; set; }

        [JsonProperty("tls_issuer")]
        public string TlsIssuer { get; set; }

        [JsonProperty("tls_subject")]
        public string TlsSubject { get; set; }

        [JsonProperty("tls_psk_identity")]
        public string TlsPskIdentity { get; set; }

        [JsonProperty("tls_psk")]
        public string TlsPsk { get; set; }
    }
}