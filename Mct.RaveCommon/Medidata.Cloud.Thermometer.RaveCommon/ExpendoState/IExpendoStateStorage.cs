using System.Collections.Generic;

namespace Medidata.Cloud.Thermometer.RaveCommon.ExpendoState
{
    public interface IExpendoStateStorage
    {
        IDictionary<string, object> GetStorage(int targetIdentity);
        void ClearStorage(int targetIdentity);
    }
}