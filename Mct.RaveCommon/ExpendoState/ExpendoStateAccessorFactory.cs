namespace Medidata.Cloud.Thermometer.RaveCommon.ExpendoState
{
    internal class ExpendoStateAccessorFactory : IExpendoStateAccessorFactory
    {
        public IExpendoStateAccessor CreateAccessor(object target, IExpendoStateStorage allStorages)
        {
            return new ExpendoStateAccessor(target, allStorages);
        }

        public IExpendoStateInstanceAccessor CreateInstanceAccessor(object target, IExpendoStateStorage allStorages,
            IExpendoStateAccessorFactory accessorFactory)
        {
            return new ExpendoStateInstanceAccessor(target, allStorages, accessorFactory);
        }
    }
}