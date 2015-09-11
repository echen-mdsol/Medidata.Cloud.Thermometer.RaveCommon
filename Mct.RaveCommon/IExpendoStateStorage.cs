using System.Collections.Generic;

namespace Medidata.Cloud.Thermometer.RaveCommon
{
    public interface IExpendoStateStorage
    {
        IDictionary<string, object> GetStorage(int targetIdentity);
        void ClearStorage(int targetIdentity);
    }
}