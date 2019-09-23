# Zabbix AD Maintenance Window Manager

This application reads the Active Directory groups for managing SCCM Maintenance Windows and makes sure that they line up in Zabbix. This will minimize on Alerts.

## Configuration File

The configuration file ***appsettings.json*** is broken into three categories:

### ADSettings

AD Settings contains the values to find the groups in AD.

* **ADOU**: This is the OU that contains all of the SCCM Mantenance Window Groups.
* **ADServer**: This is the AD server that this application will create an LDAPS connection to. This server/service must support SSL or the connection will fail.
* **ADUser**: This is the full DN for the user account that will be creating the LDAP Bind.
* **ADPassword**: This is the password for ADUser service account.

```json
"ADSettings": {
    "ADOU": "OU=Maintenance_Windows,OU=Configuration_Manager,OU=Services,OU=Managed_Groups,OU=Managed_Groups,OU=UConn,DC=grove,DC=ad,DC=uconn,DC=edu",
    "ADServer": "ldaps-grove.uits.uconn.edu",
    "ADUser": "CN=zabbix.ad.svc,OU=Accounts,OU=Zabbix,OU=UITS,OU=Services,OU=Managed_Server,OU=Managed_Server,OU=UConn,DC=grove,DC=ad,DC=uconn,DC=edu",
    "ADPassword": "[REPLACEME]"
  },
```

### MaintWinGroups

The section ***MaintWinGroups***  is an array that contains the CNs (Common Names) of all of the groups in ***ADOU*** that we will be reading the server membership from.

* **GroupName**: is the CN value of the Group. This is usually the name that is displayed in *Active Directory Users and Computers*. However to be sure you can right click on the group and select properties. The value in ***Group Name (pre-Windows 2000:)*** is the value you want to type into the config file.
* **TemplateID**: This is the ID of the matching maintenance window in Zabbix. To get this id, you need only open the Maintenance Window in Zabbix and look at the URL. You will see mainteanceid=##. The ## references the maintenance window ID.

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

### ZabbixSettings

* **Zabbix_UserName**: This is the user account that has read/write access to all of the maintenance groups and the host group that the servers are a part of. If the account does not have access to the group that the monitored computer is a port of, the change will fail with a permission denied.
* **Zabbix_Password**: This is the password for the service account **Zabbix_UserName**.
* **Zabbix_ServerName**: This is the FQDN of the Zabbix server that is monitoring the hosts in question.
* **Zabbix_DefaultID**: Not currently used but required.
* **Zabbix_JSON_RPC**: The current version of the JSON RPC. This will lock the RPC calls to what this application was coded for. This should mitigate issues with future upgrades.

```json
"ZabbixSettings": {
    "Zabbix_UserName": "its.zbxtestapi.svc",
    "Zabbix_Password": "[REPLACEME]",
    "Zabbix_ServerName": "zbx-t-web.its.uconn.edu",
    "Zabbix_DefaultID": 13,
    "Zabbix_JSON_RPC":  "2.0"
  }
```

## Complete Sample JSON File

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

## Created By

Christopher Tarricone chris@uconn.edu (c) 2019