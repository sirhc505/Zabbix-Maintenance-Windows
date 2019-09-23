using System;
using System.Collections.Generic;
using ZabbixMW.Managers;
using ZabbixMW.Models;

namespace ZabbixMW
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigurationManager configMgr = new ConfigurationManager();
            configMgr.LoadConfiguration();

            ZabbixManager zabbixManager = new ZabbixManager();
            zabbixManager.ValidateConfig();
            
            // aDManager.GetServers();

        }
    }
}
