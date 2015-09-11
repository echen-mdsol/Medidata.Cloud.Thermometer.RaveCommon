using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Medidata.Cloud.Thermometer.RaveCommon.ExpendoState
{
    internal class ExpendoStateConcurrentStorage : IExpendoStateStorage
    {
        internal readonly ConcurrentDictionary<int, Dictionary<string, object>> PropDic =
            new ConcurrentDictionary<int, Dictionary<string, object>>();

        public IDictionary<string, object> GetStorage(int targetIdentity)
        {
            return PropDic.GetOrAdd(targetIdentity, new Dictionary<string, object>());
        }

        public void ClearStorage(int targetIdentity)
        {
            Dictionary<string, object> propDic;
            PropDic.TryRemove(targetIdentity, out propDic);
        }
    }
}