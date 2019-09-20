using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Configuration;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using ZabbixMW.Managers;
using ZabbixMW.Models;
using ZabbixMW.Common;


namespace ZabbixMW.Managers
{
    class ZabbixManager
    {

        public static HttpClient ZabbixClient { get; set; }
        // If you provided the credentials correctly, the response returned by the API will contain the user authentication token.
        // This will be required for all interactions with the server.
        public static string ZabbixAuthResult { get; set; }

        private static string ZabbixServer;
        private static string ZabbixUser;
        private static string ZabbixPassword;

        public static async Task<ZabbixRpcAuthModel> GetSessionID(ZabbixRpcAuthModel zabbixRpcAuthModel)
        {
            String url = string.Format("api/api_jsonrpc.php");
            try
            {
                string postContent = JsonConvert.SerializeObject(zabbixRpcAuthModel, Formatting.Indented);
                Console.WriteLine(postContent);
                StringContent httpContent = new StringContent(postContent, Encoding.UTF8, "application/json-rpc");
                using (HttpResponseMessage response = await ZabbixClient.PostAsync(url, httpContent).ConfigureAwait(false))
                // using (HttpResponseMessage response = await ZabbixClient.PostAsJsonAsync<ZabbixRpcAuthModel>(url, zabbixRpcAuthModel).ConfigureAwait(false))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ZabbixRpcAuthResponceModel zabbixRpcAuthResponse = await response.Content.ReadAsAsync<ZabbixRpcAuthResponceModel>().ConfigureAwait(false);
                        ZabbixAuthResult = zabbixRpcAuthResponse.Result;
                        return zabbixRpcAuthModel;
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidProgramException("Failed to connect to Zabbix: " + ex.Message);

            }
        }

        public ZabbixManager()
        {

            ZabbixServer = (ConfigurationManager.ZabbixSettings.ZabbixServerName != null) ? ZabbixServer = ConfigurationManager.ZabbixSettings.ZabbixServerName : throw new InvalidProgramException("Missing Zabbix server name. Is the configuration loaded?");
            ZabbixUser = (ConfigurationManager.ZabbixSettings.ZabbixUserName != null) ? ZabbixUser = ConfigurationManager.ZabbixSettings.ZabbixUserName : throw new InvalidProgramException("Missing Zabbix user name name. Is the configuration loaded?");
            ZabbixPassword = (ConfigurationManager.ZabbixSettings.ZabbixPassword != null) ? ZabbixPassword = ConfigurationManager.ZabbixSettings.ZabbixPassword : throw new InvalidProgramException("Missing Zabbix service account password. Is the configuration loaded?");

                
            ZabbixRpcAuthParams zabbixRpcAuthParams = new ZabbixRpcAuthParams
            {
                User = ZabbixUser,
                Password = ZabbixPassword
            };
            ZabbixRpcAuthModel zabbixRpcAuthModel = new ZabbixRpcAuthModel
            {
                Jsonrpc = "2.0",
                Method = "user.login",
                Params = zabbixRpcAuthParams,
                Id = 1,
                Auth = null
            };
            try
            {
                ZabbixClient = new HttpClient();
                ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;
                ZabbixClient.DefaultRequestHeaders.Accept.Clear();
                string baseAddress = string.Format("https://{0}/", ZabbixServer);
                ZabbixClient.BaseAddress = new Uri(baseAddress);
                ZabbixClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json-rpc"));
                var setSession = GetSessionID(zabbixRpcAuthModel);
                setSession.Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

        }

        public async void SetConfig()
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
