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
        private static string ZabbixAPIUrl = "api/api_jsonrpc.php";

        public static async Task<ZabbixRpcAuthModel> GetSessionID(ZabbixRpcAuthModel zabbixRpcAuthModel)
        {
            try
            {
                string postContent = JsonConvert.SerializeObject(zabbixRpcAuthModel, Formatting.Indented);
                using (HttpResponseMessage response = await ZabbixClient.PostAsJsonAsync<ZabbixRpcAuthModel>(ZabbixAPIUrl, zabbixRpcAuthModel).ConfigureAwait(false))
                {
                    if (response.IsSuccessStatusCode)
                    {
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

        public async Task<ZabbixSearchResults> GetHostInfo(string serverName)
        {
            List<string> hostList = new List<string>();
            hostList.Add(serverName.ToLower());
            ZabbixSearcFilter zabbixSearcFilter = new ZabbixSearcFilter
            {
                Host = hostList
            };
            ZabbixSearcParams zabbixSearcParams = new ZabbixSearcParams()
            {
                Search = zabbixSearcFilter
            };
            ZabbixSearchModel zabbixSearchModel = new ZabbixSearchModel
            {
                Jsonrpc = ZabbixRPCVersion,
                Method = "host.get",
                Params = zabbixSearcParams,
                Auth = ZabbixAuthResult,
                Id = 1
            };
            try
            {
                using (HttpResponseMessage response = await ZabbixClient.PostAsJsonAsync<ZabbixSearchModel>(ZabbixAPIUrl, zabbixSearchModel).ConfigureAwait(false))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string debugResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        ZabbixSearchResultModel zabbixSearchResultModel = JsonConvert.DeserializeObject<ZabbixSearchResultModel>(debugResponse);
                        // ZabbixSearchResultModel zabbixSearchResultModel = await response.Content.ReadAsAsync<ZabbixSearchResultModel>().ConfigureAwait(false);// JsonConvert.DeserializeObject<ZabbixRpcAuthResponceModel>(authResponse);
                        if (zabbixSearchResultModel.Result != null)
                        {
                            if (zabbixSearchResultModel.Result.Count > 0)
                            {
                                return zabbixSearchResultModel.Result[0];
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            return null;
                        }

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

        public async Task<bool> UpdateMaintenanceSchedule(List<string> hostIds, int correctMaintId)
        {
            ZabbixSetHostMaintParams zabbixSetHostMaintParams = new ZabbixSetHostMaintParams
            {
                Maintenanceid = correctMaintId.ToString(),
                Hostids = hostIds
            };
            ZabbixSetHostMaintModel zabbixSetHostMaintModel = new ZabbixSetHostMaintModel
            {
                Jsonrpc = ZabbixRPCVersion,
                Method = "maintenance.update",
                Params = zabbixSetHostMaintParams,
                Auth = ZabbixAuthResult,
                Id = 1
            };
            string postContent = JsonConvert.SerializeObject(zabbixSetHostMaintModel, Formatting.Indented);
            Console.WriteLine(postContent);
            using (HttpResponseMessage response = await ZabbixClient.PostAsJsonAsync<ZabbixSetHostMaintModel>(ZabbixAPIUrl, zabbixSetHostMaintModel).ConfigureAwait(false))
            {
                if (response.IsSuccessStatusCode)
                {
                    string debugResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return true;
                }
                else
                {
                    return false;
                }
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

        public void ValidateConfig()
        {
            ADManager aDManager = new ADManager();
            List<string> serverList = new List<string>();
            List<MaintWinGroup> maintWinGroups = ConfigurationManager.maintWinGroups;
            int currentMaintID = ConfigurationManager.ZabbixSettings.ZabbixDefaultID;

            foreach (MaintWinGroup newMaintWinGroup in maintWinGroups)
            {
                serverList = aDManager.GetGroupMembership(newMaintWinGroup.GroupName);
                currentMaintID = newMaintWinGroup.TemplateId;
                List<string> serversInMaintGrp = new List<string>();
                foreach (string currentServer in serverList)
                {
                    string server = currentServer;
                    var zabbixTaskResult = GetHostInfo(server);
                    zabbixTaskResult.Wait();
                    if (zabbixTaskResult.Result != null)
                    {
                        ZabbixSearchResults zabbixSearchResult = zabbixTaskResult.Result;

                        serversInMaintGrp.Add(zabbixSearchResult.Hostid.ToString());
                    }
                    else
                    {
                        Console.WriteLine(server + " was not found in Zabbix. Make sure the names match and the host exists in Zabbix.");
                    }
                }
                if (serversInMaintGrp.Count > 0)
                {
                    Task<bool> wasUpdateSuccess = UpdateMaintenanceSchedule(serversInMaintGrp, currentMaintID);
                    if (wasUpdateSuccess.Result == true)
                    {
                        Console.WriteLine("Done!");
                    }
                    else
                    {
                        Console.WriteLine("Could not update Maintenance Window!");
                    }
                }
            }
        }
    }
}
