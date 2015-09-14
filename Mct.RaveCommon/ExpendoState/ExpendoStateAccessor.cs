using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Medidata.Cloud.Thermometer.RaveCommon.ExpendoState
{
    internal class ExpendoStateAccessor : IExpendoStateAccessor
    {
        private readonly IDictionary<string, object> _ownerStateStorage;
        protected readonly object Owner;
        protected int OwnerIdentity;

        public ExpendoStateAccessor(object owner, IExpendoStateStorage allStorages)
        {
            if (owner == null) throw new ArgumentNullException("owner");
            if (allStorages == null) throw new ArgumentNullException("allStorages");
            OwnerIdentity = RuntimeHelpers.GetHashCode(owner);
            Owner = owner;
            _ownerStateStorage = allStorages.GetStorage(OwnerIdentity);
        }

        public virtual IExpendoStateAccessor Set(string name, object value)
        {
            if (_ownerStateStorage.ContainsKey(name))
            {
                _ownerStateStorage[name] = value;
            }
            else
            {
                _ownerStateStorage.Add(name, value);
            }
            return this;
        }

        public virtual object Get(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Expendo property name cannot be empty.", "name");

            return _ownerStateStorage[name];
        }

        public virtual IExpendoStateAccessor Remove(string name)
        {
            _ownerStateStorage.Remove(name);
            return this;
        }

        public IExpendoStateAccessor RemoveAll()
        {
            _ownerStateStorage.Clear();
            return this;
        }

        public virtual bool Exists(string name)
        {
            return _ownerStateStorage.ContainsKey(name);
        }

        public IEnumerable<string> Keys
        {
            get { return _ownerStateStorage.Keys; }
        }
    }
}