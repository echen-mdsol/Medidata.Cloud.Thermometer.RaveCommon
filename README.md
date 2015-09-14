# Medidata.Cloud.Thermometer.RaveCommon
This contains several common classes for 5 Rave components (web, rws, reporting, core, and riss) to use the Thermometer.

# Features
Please refer to below feature files.
- [ExpendoStateServiceForClass.feature](Mct.RaveCommon.Specs/ExpendoStateServiceForClass.feature)
-  [ExpendoStateServiceForInstance.feature](Mct.RaveCommon.Specs/ExpendoStateServiceForInstance.feature)
- [AbandonExpendoStateWhenInstanceIsToBeGarbageCollected.feature](Mct.RaveCommon.Specs/AbandonExpendoStateWhenInstanceIsToBeGarbageCollected.feature)

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

### Static expendo states for static classes
```cs
// Set state
var stateService = DIContainer.Resolve<IExpendoStateService>();

stateService.ForClass(typeof(StaticClass)).Set("State", "Running");

// Get state
var value = stateService.ForClass(typeof(StaticClass)).Get("State");
// value is "Running"
```

### Static expendo states for instances
Refer to below sample code to manipulate instances' static expendo states.
```cs
var cat = new Mammal();
var mouse = new Mammal();

stateService.ForInstance(cat).Static.Set("Action", "Sleeping");
stateService.ForInstance(mouse).Static.Set("Action", "Eating");
var catDoes = stateService.ForInstance(cat).Static.Get("Action");
// cat is "Eating"
var mouseDoes = stateService.ForInstance(cat).Static.Get("Action");
// mouse is "Eating"
```
You can also use `cat.GetType()` or directly `Mammal` class to do the same thing
```cs
stateService.ForInstance(cat).Static
// is equivalent to
stateService.ForClass(cat.GetType())
// is equivalent to
stateService.ForClass<Mammal>()
```

### Instance expendo states
```cs
var cat = new Mammal();
var mouse = new Mammal();

stateService.ForInstance(cat).Set("Name", "Tom");
stateService.ForInstance(mouse).Set("Name", "Jerry");

var catName = stateService.ForInstance(cat).Get("Name");
// catName is "Tom"

var mouseName = stateService.ForInstance(mouse).Get("Name");
// mouseName is "Jerry"
```

#### GC instance's expendo states
Expendo states are like extra assets for instances. An instance might be garbage collected by .NET framework. If the expendo states are required to be cleared up together with their owner instance's death, you should consider making these expendo states GC-able.

To achieve this, you must call `Abandon()` within the instance's destructor. See below sample code for `class Mammal`. In this way, all expendo state objects of the instance will be handled by the next round GC.
```cs
public class Mammal {
    // ...
    ~Mammal(){
        DIContainer.Resolve<IExpendoStateService>().ForInstance(this).Abandon();
    }
}
```
