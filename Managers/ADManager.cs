using System;
using System.Collections.Generic;
using System.Text;
using ZabbixMW.Managers;
using ZabbixMW.Models;
using ZabbixMW.Common;
using Microsoft.Extensions.Configuration;
using System.DirectoryServices;
using System.IO;
using System.Linq;

namespace ZabbixMW.Managers
{
    class ADManager
    {        

        public List<string> GetGroupMembership(string groupName)
        {
            AdSettings adSettings = ConfigurationManager.AdSettings;
            if (adSettings.AdServer != null)
            {
                try
                {
                    string ldapDN = string.Format("LDAP://{0}:636/CN={1},{2}", adSettings.AdServer, groupName, adSettings.Adou);



                    DirectoryEntry directoryEntry = new DirectoryEntry(ldapDN, adSettings.AdUser, adSettings.AdPassword, AuthenticationTypes.SecureSocketsLayer);
                    List<string> servers = new List<string>();
                    string cn = null;
                    foreach (object dn in directoryEntry.Properties["member"])
                    {
                        if (dn != null)
                        {
                            string distinguishedName = dn.ToString();
                            cn = distinguishedName.Split(',').Where(i => i.Contains("CN=")).Select(i => i.Replace("CN=", "")).FirstOrDefault();
                        }
                        string serverName = string.Format("{0}.grove.ad.uconn.edu", cn.ToUpper());
                        servers.Add(serverName);
                    }

                    return servers;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Tried to open group: [" + groupName + "]: Error: " + ex.Message);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
