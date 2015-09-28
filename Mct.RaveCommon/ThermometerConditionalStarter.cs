using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Medidata.Cloud.Thermometer.RaveCommon
{
    /// <summary>
    ///     Thermometer conditional starter. Decorator pattern.
    /// </summary>
    public class ThermometerConditionalStarter : IThermometerStarter, IDisposable
    {
        private readonly bool _conditional;
        private readonly IThermometerStarter _starter;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="conditional">If true start, false not start.</param>
        /// <param name="starter">The real starter.</param>
        public ThermometerConditionalStarter(bool conditional, IThermometerStarter starter)
        {
            if (starter == null) throw new ArgumentNullException("starter");
            _conditional = conditional;
            _starter = starter;
        }

        /// <summary>
        ///     Dispose method which does nothing indeed.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            // Does nothing.
        }

        /// <summary>
        ///     Start the concrete starter only when the conditional is true.
        /// </summary>
        /// <returns>IDisposable.</returns>
        public IDisposable Start()
        {
            return _conditional ? _starter.Start() : this;
        }

        /// <summary>
        ///     Start the concrete starter asynchronously only when the conditional is true.
        /// </summary>
        /// <returns>Task of IDisposable.</returns>
        public Task<IDisposable> StartAsync()
        {
            if (_conditional) return _starter.StartAsync();
            var tcs = new TaskCompletionSource<IDisposable>();
            tcs.SetResult(this);
            return tcs.Task;
        }
    }
}