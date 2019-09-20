using System;
using System.Collections.Generic;
using System.Net;
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
        private static string ZabbixRPCVersion;

        public static async Task<ZabbixRpcAuthModel> GetSessionID(ZabbixRpcAuthModel zabbixRpcAuthModel)
        {
            String url = string.Format("api/api_jsonrpc.php");
            try
            {
                string postContent = JsonConvert.SerializeObject(zabbixRpcAuthModel, Formatting.Indented);
                StringContent httpContent = new StringContent(postContent, Encoding.UTF8, "application/json");
                using (HttpResponseMessage response = await ZabbixClient.PostAsJsonAsync<ZabbixRpcAuthModel>(url,zabbixRpcAuthModel).ConfigureAwait(false))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        // string authResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);   // .ReadAsync<ZabbixRpcAuthModel>().ConfigureAwait(false);
                        ZabbixRpcAuthResponceModel zabbixRpcAuthResponceModel = await response.Content.ReadAsAsync<ZabbixRpcAuthResponceModel>().ConfigureAwait(false);// JsonConvert.DeserializeObject<ZabbixRpcAuthResponceModel>(authResponse);
                        ZabbixAuthResult = zabbixRpcAuthResponceModel.Result;
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
            ZabbixRPCVersion = (ConfigurationManager.ZabbixSettings.ZabbixJsonRpc != null) ? ZabbixRPCVersion = ConfigurationManager.ZabbixSettings.ZabbixJsonRpc : throw new InvalidProgramException("Missing Zabbix RPC version number. Is the configuration loaded?");

            ZabbixRpcAuthParams zabbixRpcAuthParams = new ZabbixRpcAuthParams
            {
                User = ZabbixUser,
                Password = ZabbixPassword
            };
            ZabbixRpcAuthModel zabbixRpcAuthModel = new ZabbixRpcAuthModel
            {
                Jsonrpc = ZabbixRPCVersion,
                Method = "user.login",
                Params = zabbixRpcAuthParams,
                Id = 1,
                Auth = null
            };
            try
            {
                ZabbixClient = new HttpClient();

                string authCreds = string.Format("{0}:{1}", ZabbixUser, ZabbixPassword);
                var byteArray = Encoding.ASCII.GetBytes(authCreds);

                ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;
                ZabbixClient.DefaultRequestHeaders.Accept.Clear();
                string baseAddress = string.Format("https://{0}/", ZabbixServer);
                ZabbixClient.BaseAddress = new Uri(baseAddress);
                ZabbixClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                ZabbixClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var setSession = GetSessionID(zabbixRpcAuthModel);
                setSession.Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
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
