# Medidata.Cloud.Thermometer.RaveCommon
This contains several common classes for 5 Rave components (web, rws, reporting, core, and riss) to use the Thermometer.

# Features
Please refer to below feature files.
- [ExpendoStateServiceForClass.feature](Medidata.Cloud.Thermometer.RaveCommon.Specs/ExpendoStateServiceForClass.feature)
-  [ExpendoStateServiceForInstance.feature](Medidata.Cloud.Thermometer.RaveCommon.Specs/ExpendoStateServiceForInstance.feature)

# Classes
## `ExpendoStateService`
To diagnose the state of certain services or threads in a Rave component on the fly, we need these services to be able to notify their state changes to the outside state collector. For code legacy issue, we try to avoid polluting Rave codebase as possible. To achieve this, we encourage developers to use `ExpendoStateService` to collect the state information.

### Inject `ExpendoStateService` as singleton via unity
`ExpendoStateService` itself wasn't designed as a singleton instance. But to keep correct tracking states of static classes, we highly recommend that within each Rave component `ExpendoStateService` should be singleton. This can usually simply achieved by unity dependency injection framework.

```xml
<!-- unity.config -->
<register type="Medidata.Cloud.Thermometer.RaveCommon.IExpendoStateService, Medidata.Cloud.Thermometer.RaveCommon"
        mapTo="Medidata.Cloud.Thermometer.RaveCommon.ExpendoStateService, Medidata.Cloud.Thermometer.RaveCommon">
      <lifetime type="singleton" />
</register>      
```

### `ExpendoStateService` for static classes
```cs
// Set state
var stateService = DIContainer.Resolve<IExpendoStateService>();
stateService.ForClass<StaticClass>().Set("State", "Running");

// Get state
var value = stateService.ForClass<StaticClass>().Get("State");
Assert.AreEqual("Running", value);
```
Or you can use equivalent non-generic way as below.
```cs
// Set state
var stateService = DIContainer.Resolve<IExpendoStateService>();
stateService.ForClass(typeof(StaticClass)).Set("State", "Running");

// Get state
var value = stateService.ForClass(typeof(StaticClass)).Get("State");
Assert.AreEqual("Running", value);
```

### `ExpendoStateService` for instances
```cs
var cat, mouse;
var stateService = DIContainer.Resolve<IExpendoStateService>();

stateService.ForInstance(cat).Set("Name", "Tom");
stateService.ForInstance(mouse).Set("Name", "Jerry");

var catName = stateService.ForInstance(cat).Get("Name");
var mouseName = stateService.ForInstance(mouse).Get("Name");

Assert.AreEqual("Tom", catName);
Assert.AreEqual("Jerry", mouseName);
```
