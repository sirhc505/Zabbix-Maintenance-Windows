using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using ZabbixMW.Managers;
using ZabbixMW.Models;
using ZabbixMW.Common;


namespace ZabbixMW.Managers
{
    class ZabbixManager
    {

        public static HttpClient ZabbixClient { get; set; }

        public bool InitializeClient(string zabbixServer, string zabbixUser, string zabbixPassword)
        {
            try
            {
                
                ZabbixClient = new HttpClient();
                ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;
                ZabbixClient.DefaultRequestHeaders.Accept.Clear();

                string baseAddress = string.Format("https://{0}/api", zabbixServer);
                ZabbixClient.BaseAddress = new Uri(baseAddress);
                ZabbixClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string authCreds = string.Format("{0}:{1}", zabbixUser, zabbixPassword);
                var byteArray = Encoding.ASCII.GetBytes(authCreds);
                ZabbixClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                return true;
            }
            catch
            {
                return false;
            }

        }

        public void SetConfig()
        {
            ADManager aDManager = new ADManager();
            List<string> serverList = new List<string>();
            List<MaintWinGroup> maintWinGroups = ConfigurationManager.maintWinGroups;
            int currentMaintID = ConfigurationManager.ZabbixSettings.ZabbixDefaultID;
            int defaultMaintID = ConfigurationManager.ZabbixSettings.ZabbixDefaultID;


            foreach (MaintWinGroup newMaintWinGroup in maintWinGroups)
            {
                serverList = aDManager.GetGroupMembership(newMaintWinGroup.GroupName);
                currentMaintID = newMaintWinGroup.TemplateId;
                foreach (string server in serverList)
                {

                }
            }


        }

        public int GetHostCurrentMaintenanceID (string serverName)
        {


            return 0;
        }


    }
}
