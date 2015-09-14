using System;

namespace Medidata.Cloud.Thermometer.RaveCommon.ExpendoState
{
    internal class ExpendoStateInstanceAccessor : ExpendoStateAccessor, IExpendoStateInstanceAccessor
    {
        private readonly IExpendoStateAccessorFactory _accessorFactory;
        private readonly IExpendoStateStorage _allStorages;

        public ExpendoStateInstanceAccessor(object owner, IExpendoStateStorage allStorages,
            IExpendoStateAccessorFactory accessorFactory) : base(owner, allStorages)
        {
            if (accessorFactory == null) throw new ArgumentNullException("accessorFactory");
            _allStorages = allStorages;
            _accessorFactory = accessorFactory;
        }

        public void Abandon()
        {
            _allStorages.AbandonStorage(OwnerIdentity);
        }

        public IExpendoStateAccessor Static
        {
            get { return _accessorFactory.CreateAccessor(Owner.GetType(), _allStorages); }
        }
    }
}