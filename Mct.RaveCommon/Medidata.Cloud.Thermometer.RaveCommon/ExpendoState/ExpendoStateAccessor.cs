using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Medidata.Cloud.Thermometer.RaveCommon.ExpendoState
{
    internal class ExpendoStateAccessor : IExpendoStateAccessor
    {
        private readonly IDictionary<string, object> _stateStorage;

        public ExpendoStateAccessor(object target, IExpendoStateStorage stateStorage)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (stateStorage == null) throw new ArgumentNullException("stateStorage");
            var identity = RuntimeHelpers.GetHashCode(target);
            _stateStorage = stateStorage.GetStorage(identity);
        }

        public virtual IExpendoStateAccessor Set(string name, object value)
        {
            if (_stateStorage.ContainsKey(name))
            {
                _stateStorage[name] = value;
            }
            else
            {
                _stateStorage.Add(name, value);
            }
            return this;
        }

        public virtual object Get(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Expendo property name cannot be empty.", "name");

            return _stateStorage[name];
        }

        public virtual IExpendoStateAccessor Remove(string name)
        {
            _stateStorage.Remove(name);
            return this;
        }

        public IExpendoStateAccessor RemoveAll()
        {
            _stateStorage.Clear();
            return this;
        }

        public virtual bool Exists(string name)
        {
            return _stateStorage.ContainsKey(name);
        }

        public IEnumerable<string> Keys
        {
            get { return _stateStorage.Keys; }
        }
    }
}