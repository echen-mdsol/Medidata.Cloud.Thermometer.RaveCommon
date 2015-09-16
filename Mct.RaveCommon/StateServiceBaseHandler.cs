using System;
using System.Diagnostics.CodeAnalysis;
using Medidata.Cloud.StateBroker;

namespace Medidata.Cloud.Thermometer.RaveCommon
{
    /// <summary>
    ///     Abstract base handler with a state service injected via constructor parameter.
    /// </summary>
    public abstract class StateServiceBaseHandler : ThermometerBaseHandler
    {
        private readonly IStateService _stateService;

        protected StateServiceBaseHandler(IStateService stateService)
        {
            if (stateService == null) throw new ArgumentNullException("stateService");
            _stateService = stateService;
        }

        protected override object HandleQuestion(IThermometerQuestion question)
        {
            return HandleQuestion(question, _stateService);
        }

        protected abstract object HandleQuestion(IThermometerQuestion question, IStateService stateService);
    }
}