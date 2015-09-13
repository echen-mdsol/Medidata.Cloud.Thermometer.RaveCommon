using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Medidata.Cloud.Thermometer.RaveCommon.ExpendoState
{
    internal class ExpendoStateAccessor : IExpendoStateAbandonableAccessor
    {
        private readonly IExpendoStateStorage _stateStorageCompany;
        protected readonly IDictionary<string, object> StateStorage;
        private int _identity;

        public ExpendoStateAccessor(object target, IExpendoStateStorage stateStorageCompany)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (stateStorageCompany == null) throw new ArgumentNullException("stateStorageCompany");
            _identity = RuntimeHelpers.GetHashCode(target);
            _stateStorageCompany = stateStorageCompany;
            StateStorage = _stateStorageCompany.GetStorage(_identity);
        }

        public virtual IExpendoStateAccessor Set(string name, object value)
        {
            if (StateStorage.ContainsKey(name))
            {
                StateStorage[name] = value;
            }
            else
            {
                StateStorage.Add(name, value);
            }
            return this;
        }

        public virtual object Get(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Expendo property name cannot be empty.", "name");

            return StateStorage[name];
        }

        public virtual IExpendoStateAccessor Remove(string name)
        {
            StateStorage.Remove(name);
            return this;
        }

        public IExpendoStateAccessor RemoveAll()
        {
            StateStorage.Clear();
            return this;
        }

        public virtual bool Exists(string name)
        {
            return StateStorage.ContainsKey(name);
        }

        public void Abandon()
        {
            _stateStorageCompany.AbandonStorage(_identity);
        }

        public IEnumerable<string> Keys
        {
            get { return StateStorage.Keys; }
        }
    }
}