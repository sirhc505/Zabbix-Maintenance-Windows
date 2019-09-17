using System;
using System.Collections.Generic;
using System.Text;

namespace ZabbixMW.Models
{
    public partial class ZabbixADC
    {
        public string Name { get; set; }
        public string IPAddress { get; set; }
        public string OSVersion { get; set; }
    }

    public class ZabbixDomainControllers
    {
        public List<ZabbixADC> Ucadc { get; set; }
    }
}
