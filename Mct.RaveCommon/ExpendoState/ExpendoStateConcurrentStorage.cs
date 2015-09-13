using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Medidata.Cloud.Thermometer.RaveCommon.ExpendoState
{
    internal class ExpendoStateConcurrentStorage : IExpendoStateStorage
    {
        internal readonly ConcurrentDictionary<int, Dictionary<string, object>> AllStorages =
            new ConcurrentDictionary<int, Dictionary<string, object>>();

        public IDictionary<string, object> GetStorage(int targetIdentity)
        {
            return AllStorages.GetOrAdd(targetIdentity, new Dictionary<string, object>());
        }

        public void ReleaseStorage(int targetIdentity)
        {
            Dictionary<string, object> propDic;
            AllStorages.TryRemove(targetIdentity, out propDic);
        }
    }
}