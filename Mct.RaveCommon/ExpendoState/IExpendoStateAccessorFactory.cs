using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Medidata.Cloud.Thermometer.RaveCommon.ExpendoState
{
    internal interface IExpendoStateAccessorFactory
    {
        IExpendoStateAccessor CreateAccessor(object target, IExpendoStateStorage allStorages);
        IExpendoStateInstanceAccessor CreateInstanceAccessor(object target, IExpendoStateStorage allStorages, IExpendoStateAccessorFactory accessorFactory);
    }
}
