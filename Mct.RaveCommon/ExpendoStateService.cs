using System;
using Medidata.Cloud.Thermometer.RaveCommon.ExpendoState;

namespace Medidata.Cloud.Thermometer.RaveCommon
{
    public class ExpendoStateService : IExpendoStateService
    {
        private readonly IExpendoStateStorage _stateStorage;

        public ExpendoStateService(IExpendoStateStorage stateStorage = null)
        {
            _stateStorage = stateStorage ?? new ExpendoStateConcurrentStorage();
        }

        public virtual IExpendoStateAccessor ForInstance(object instance)
        {
            if (instance == null) throw new ArgumentNullException("instance");
            if (instance is Type || instance is string)
                throw new NotSupportedException(string.Format("Instance of '{0}' isn't supported",
                    instance.GetType().FullName));

            return new ExpendoStateAccessor(instance, _stateStorage);
        }

        public virtual IExpendoStateAccessor ForClass<T>() where T : class
        {
            return ForClass(typeof (T));
        }

        public IExpendoStateAccessor ForClass(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            return new ExpendoStateAccessor(type, _stateStorage);
        }
    }
}