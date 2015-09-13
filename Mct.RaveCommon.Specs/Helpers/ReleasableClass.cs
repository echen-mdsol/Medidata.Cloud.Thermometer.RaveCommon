using System;

namespace Medidata.Cloud.Thermometer.RaveCommon.Specs.Helpers
{
    internal class ReleasableClass
    {
        private readonly IExpendoStateService _expendoStateService;

        public ReleasableClass(IExpendoStateService expendoStateService)
        {
            if (expendoStateService == null) throw new ArgumentNullException("expendoStateService");
            _expendoStateService = expendoStateService;
        }

        ~ReleasableClass()
        {
            _expendoStateService.ForInstance(this).Release();
        }
    }
}