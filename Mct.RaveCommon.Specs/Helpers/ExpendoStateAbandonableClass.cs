using System;

namespace Medidata.Cloud.Thermometer.RaveCommon.Specs.Helpers
{
    internal class ExpendoStateAbandonableClass
    {
        private readonly IExpendoStateService _expendoStateService;

        public ExpendoStateAbandonableClass(IExpendoStateService expendoStateService)
        {
            if (expendoStateService == null) throw new ArgumentNullException("expendoStateService");
            _expendoStateService = expendoStateService;
        }

        ~ExpendoStateAbandonableClass()
        {
            _expendoStateService.ForInstance(this).Abandon();
        }
    }
}