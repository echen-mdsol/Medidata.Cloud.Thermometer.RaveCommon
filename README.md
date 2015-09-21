# Medidata.Cloud.Thermometer.RaveCommon
This contains several common classes for 5 Rave components (web, rws, reporting, core, and riss) to use the Thermometer.

---

#### `AssemblyInfoHandler`
##### Route path
`/assemblyinfo`
##### Function
Lists all assembly version information.
##### Sample answer (response):
```json
{
    "mscorlib": "4.0.0.0",
    "Medidata.Rave.Integration.Service": "5.6.5.202",
    "System.ServiceProcess": "4.0.0.0",
    "System": "4.0.0.0",
    "System.Core": "4.0.0.0",
    "System.Configuration.Install": "4.0.0.0",
    "Medidata.Rave.Integration.Common": "5.6.5.202",
    "Medidata.Logging": "5.6.5.202",
    "Medidata.Interfaces": "1.0.0.0",
    "Medidata.Rave.Integration.Interfaces": "5.6.5.202"
}
```
---

#### `ComponentInfoHandler`
##### Route path
`/componentinfo`
##### Function
Gets product component information.
##### Sample answer (response):
```json
{
    "product": "Medidata Rave © 2015.3.0",
    "component": "Medidata.Core.Service",
    "productVersion": "5.6.5.202",
    "buildId": "YYYYMMDD-githash"
}
```
---
#### `DbInfoHandler`
##### Route path
`/dbinfo`
##### Function
Gets component database connection information.
##### Sample answer (response):
```json
{
   "CachingTimeOut": 10,
   "DefaultHint": "RaveDev",
   "ConnectionSettings": [
      {
         "DataSourceHint": "RaveDev",
         "ServerName": "WIN81",
         "DatabaseName": "RaveDev",
         "ProviderType": "SqlClient",
         "MaxPoolSize": 100,
         "MultipleActiveResultSets": false,
         "ConnectionTimeOut": 600,
         "CanConnect": "Yes"
      },
      {
         "DataSourceHint": "RaveDev_Reporter",
         "ServerName": "WIN81",
         "DatabaseName": "RaveDev",
         "ProviderType": "SqlClient",
         "MaxPoolSize": 300,
         "MultipleActiveResultSets": true,
         "ConnectionTimeOut": 600,
         "CanConnect": "Yes"
      },
      {
         "DataSourceHint": "RaveDev_Migration",
         "ServerName": "WIN81",
         "DatabaseName": "RaveDev",
         "ProviderType": "SqlClient",
         "MaxPoolSize": 300,
         "MultipleActiveResultSets": true,
         "ConnectionTimeOut": 3000,
         "CanConnect": "Yes"
      }
   ]
}
```
