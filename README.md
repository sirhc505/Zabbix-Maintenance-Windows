# Zabbix AD Maintenance Window Manager

- [Zabbix AD Maintenance Window Manager](#zabbix-ad-maintenance-window-manager)
  - [1.1. Control Logic](#11-control-logic)
  - [System Requirements](#system-requirements)
    - [Running](#running)
    - [Building](#building)
  - [1.2. Configuration File](#12-configuration-file)
    - [1.2.1. ADSettings](#121-adsettings)
    - [1.2.2. MaintWinGroups](#122-maintwingroups)
    - [1.2.3. ZabbixSettings](#123-zabbixsettings)
  - [1.3. Complete Sample JSON File](#13-complete-sample-json-file)
  - [1.4. Created By](#14-created-by)

This application reads the Active Directory groups for managing SCCM Maintenance Windows and makes sure that they line up in Zabbix. This will minimize on Alerts.

## 1.1. Control Logic

This application assume that there is a default maintenance window that corresponds with ITS' current maintenance window policy. If a server is added to one of the groups in [MaintWinGroups](#122-maintwingroups) then after this script runs, it will be part of that group. If the server is removed from a group, it will then be removed from the Zabbix maintenance window as well.

Zabbix re-writes the entire group membership when you call [maintenance.update](https://www.zabbix.com/documentation/4.2/manual/api/reference/maintenance/update) through the API. This can cause potential problems if they are managed outside of the API or manually through the use of the Web UI.

## System Requirements

### Running

- Windows Server 2106 or Windows 10 or Later
- x64 based operating system
- Microsoft .NEt 4.7.2 framework or later.

### Building

- Windows 10 x64 or Later
- Visual Studio 2019
- Microsoft .NEt 4.7.2 framework
- The following NuGet Packages
  - Newtonsoft.Json
  - System.DirectoryServices (+ .Protocols)
  - System.Security.Cryptograpghy.OpenSsl
  - System.Net.Http
  - Microsoft.Extensions.Configuration (+ .Binder, .FileExtensions, .Json)
  - Microsoft.AspNet.WebApi.Client

## 1.2. Configuration File

The configuration file ***appsettings.json*** is broken into three categories:

### 1.2.1. ADSettings

AD Settings contains the values to find the groups in AD.

- **ADOU**: This is the OU that contains all of the SCCM Maintenance Window Groups.
- **ADServer**: This is the AD server that this application will create an LDAPS connection to. This server/service must support SSL or the connection will fail.
- **ADUser**: This is the full DN for the user account that will be creating the LDAP Bind.
- **ADPassword**: This is the password for ADUser service account.

```json
"ADSettings": {
    "ADOU": "OU=Maintenance_Windows,OU=Configuration_Manager,OU=Services,OU=Managed_Groups,OU=Managed_Groups,OU=UConn,DC=grove,DC=ad,DC=uconn,DC=edu",
    "ADServer": "ldaps-grove.uits.uconn.edu",
    "ADUser": "CN=zabbix.ad.svc,OU=Accounts,OU=Zabbix,OU=UITS,OU=Services,OU=Managed_Server,OU=Managed_Server,OU=UConn,DC=grove,DC=ad,DC=uconn,DC=edu",
    "ADPassword": "[REPLACEME]"
  },
```

### 1.2.2. MaintWinGroups

The section ***MaintWinGroups***  is an array that contains the CNs (Common Names) of all of the groups in ***ADOU*** that we will be reading the server membership from.

- **GroupName**: is the CN value of the Group. This is usually the name that is displayed in *Active Directory Users and Computers*. However to be sure you can right click on the group and select properties. The value in ***Group Name (pre-Windows 2000:)*** is the value you want to type into the config file.
- **TemplateID**: This is the ID of the matching maintenance window in Zabbix. To get this id, you need only open the Maintenance Window in Zabbix and look at the URL. You will see mainteanceid=##. The ## references the maintenance window ID.

```json
"MaintWinGroups": [
    {
      "GroupName": "UITS.SSG.CM.MAINTWIN.11PM",
      "TemplateID": 4
    },
    {
      "GroupName": "UITS.SSG.CM.MAINTWIN.1AM",
      "TemplateID": 1
    },
    {
      "GroupName": "UITS.SSG.CM.MAINTWIN.3AM",
      "TemplateID": 2
    },
    {
      "GroupName": "UITS.SSG.CM.MAINTWIN.8PM",
      "TemplateID": 3
    },
    {
      "GroupName": "UITS.SSG.CM.MAINTWIN.MON.1AM",
      "TemplateID": 5
    },
    {
      "GroupName": "UITS.SSG.CM.MAINTWIN.SAT",
      "TemplateID": 6
    },
    {
      "GroupName": "UITS.SSG.CM.MAINTWIN.SAT.8-12PM",
      "TemplateID": 7
    },
    {
      "GroupName": "UITS.SSG.CM.MAINTWIN.SUN",
      "TemplateID": 8
    },
    {
      "GroupName": "UITS.SSG.CM.MAINTWIN.SUN.8-12PM",
      "TemplateID": 9
    },
    {
      "GroupName": "UITS.SSG.CM.MAINTWIN.TUES.11PM-3AM",
      "TemplateID": 11
    },
    {
      "GroupName": "UITS.SSG.CM.MAINTWIN.TUES.7PM-11PM",
      "TemplateID": 10
    },
    {
      "GroupName": "UITS.SSG.CM.MAINTWIN.WED.3AM-7AM",
      "TemplateID": 12
    }
  ]
```

### 1.2.3. ZabbixSettings

- **Zabbix_UserName**: This is the user account that has read/write access to all of the maintenance groups and the host group that the servers are a part of. If the account does not have access to the group that the monitored computer is a port of, the change will fail with a permission denied.
- **Zabbix_Password**: This is the password for the service account **Zabbix_UserName**.
- **Zabbix_ServerName**: This is the FQDN of the Zabbix server that is monitoring the hosts in question.
- **Zabbix_DefaultID**: Not currently used but required.
- **Zabbix_JSON_RPC**: The current version of the JSON RPC. This will lock the RPC calls to what this application was coded for. This should mitigate issues with future upgrades.

```json
"ZabbixSettings": {
    "Zabbix_UserName": "its.zbxtestapi.svc",
    "Zabbix_Password": "[REPLACEME]",
    "Zabbix_ServerName": "zbx-t-web.its.uconn.edu",
    "Zabbix_DefaultID": 13,
    "Zabbix_JSON_RPC":  "2.0"
  }
```

## 1.3. Complete Sample JSON File

```json
{
  "ADSettings": {
    "ADOU": "OU=Maintenance_Windows,OU=Configuration_Manager,OU=Services,OU=Managed_Groups,OU=Managed_Groups,OU=UConn,DC=grove,DC=ad,DC=uconn,DC=edu",
    "ADServer": "ldaps-grove.uits.uconn.edu",
    "ADUser": "CN=zabbix.ad.svc,OU=Accounts,OU=Zabbix,OU=UITS,OU=Services,OU=Managed_Server,OU=Managed_Server,OU=UConn,DC=grove,DC=ad,DC=uconn,DC=edu",
    "ADPassword": "[REPLACEME]"
  },
  "MaintWinGroups": [
    {
      "GroupName": "UITS.SSG.CM.MAINTWIN.11PM",
      "TemplateID": 4
    },
    {
      "GroupName": "UITS.SSG.CM.MAINTWIN.1AM",
      "TemplateID": 1
    },
    {
      "GroupName": "UITS.SSG.CM.MAINTWIN.3AM",
      "TemplateID": 2
    },
    {
      "GroupName": "UITS.SSG.CM.MAINTWIN.8PM",
      "TemplateID": 3
    },
    {
      "GroupName": "UITS.SSG.CM.MAINTWIN.MON.1AM",
      "TemplateID": 5
    },
    {
      "GroupName": "UITS.SSG.CM.MAINTWIN.SAT",
      "TemplateID": 6
    },
    {
      "GroupName": "UITS.SSG.CM.MAINTWIN.SAT.8-12PM",
      "TemplateID": 7
    },
    {
      "GroupName": "UITS.SSG.CM.MAINTWIN.SUN",
      "TemplateID": 8
    },
    {
      "GroupName": "UITS.SSG.CM.MAINTWIN.SUN.8-12PM",
      "TemplateID": 9
    },
    {
      "GroupName": "UITS.SSG.CM.MAINTWIN.TUES.11PM-3AM",
      "TemplateID": 11
    },
    {
      "GroupName": "UITS.SSG.CM.MAINTWIN.TUES.7PM-11PM",
      "TemplateID": 10
    },
    {
      "GroupName": "UITS.SSG.CM.MAINTWIN.WED.3AM-7AM",
      "TemplateID": 12
    }
  ],
  "ZabbixSettings": {
    "Zabbix_UserName": "its.zbxtestapi.svc",
    "Zabbix_Password": "[REPLACEME]",
    "Zabbix_ServerName": "zbx-t-web.its.uconn.edu",
    "Zabbix_DefaultID": 13,
    "Zabbix_JSON_RPC":  "2.0"
  }
}
```

## 1.4. Created By

Christopher Tarricone chris@uconn.edu (c) 2019
