using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Text;
using ZabbixMW.Models;
using System.Linq;

namespace ZabbixMW.Managers
{
    // public IConfiguration Configuration { get; set; }

    class ConfigurationManager
    {

        public IConfiguration _configuration { get; set;}
        public static List<MaintWinGroup> maintWinGroups = new List<MaintWinGroup>();
        public static AdSettings AdSettings = new AdSettings();
        public static ZabbixSettings ZabbixSettings = new ZabbixSettings();

        public void LoadConfiguration()
        {

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            _configuration = config.Build();

            IConfigurationSection _adconfig = _configuration.GetSection("ADSettings");

            AdSettings.AdServer = _adconfig.GetValue<string>("ADServer");
            AdSettings.AdUser = _adconfig.GetValue<string>("ADUser");
            AdSettings.AdPassword = _adconfig.GetValue<string>("ADPassword");
            AdSettings.Adou = _adconfig.GetValue<string>("ADOU");

            IConfigurationSection _zabbixconfig = _configuration.GetSection("ZabbixSettings");
            ZabbixSettings.ZabbixServerName = _zabbixconfig.GetValue<string>("Zabbix_ServerName");
            ZabbixSettings.ZabbixUserName = _zabbixconfig.GetValue<string>("Zabbix_UserName");
            ZabbixSettings.ZabbixPassword = _zabbixconfig.GetValue<string>("Zabbix_Password");

            IConfigurationSection confMaintWinGroups = _configuration.GetSection("MaintWinGroups");
            foreach (IConfigurationSection configurationSection in confMaintWinGroups.GetChildren())
            {
                MaintWinGroup newMaintWinGroup = new MaintWinGroup();
                newMaintWinGroup.GroupName = configurationSection.GetValue<string>("GroupName");
                newMaintWinGroup.TemplateId = configurationSection.GetValue<int>("TemplateID");
                maintWinGroups.Add(newMaintWinGroup);
            }
        }
    }
}
