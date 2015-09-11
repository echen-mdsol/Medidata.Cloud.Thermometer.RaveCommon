using System;

namespace Medidata.Cloud.Thermometer.RaveCommon.ExpendoState
{
    public interface IExpendoStateService
    {
        IExpendoStateAccessor ForInstance(object instance);
        IExpendoStateAccessor ForClass<T>() where T : class;
        IExpendoStateAccessor ForClass(Type type);
    }
}