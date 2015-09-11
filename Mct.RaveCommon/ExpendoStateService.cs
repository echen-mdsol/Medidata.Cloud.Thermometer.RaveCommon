using System;
using Medidata.Cloud.Thermometer.RaveCommon.ExpendoState;

namespace Medidata.Cloud.Thermometer.RaveCommon
{
    /// <summary>
    ///     Expendo state service.
    /// </summary>
    public class ExpendoStateService : IExpendoStateService
    {
        private readonly IExpendoStateStorage _stateStorage;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="stateStorage">An implementation of state storage.</param>
        public ExpendoStateService(IExpendoStateStorage stateStorage = null)
        {
            _stateStorage = stateStorage ?? new ExpendoStateConcurrentStorage();
        }

        /// <summary>
        ///     Gets the accessor to operate expendo state for the specified instance.
        /// </summary>
        /// <param name="instance">Object instance.</param>
        /// <returns>The expendo state accessor.</returns>
        public virtual IExpendoStateAccessor ForInstance(object instance)
        {
            if (instance == null) throw new ArgumentNullException("instance");
            if (instance is Type || instance is string)
                throw new NotSupportedException(string.Format("Instance of '{0}' isn't supported",
                    instance.GetType().FullName));

            return new ExpendoStateAccessor(instance, _stateStorage);
        }

        /// <summary>
        ///     Gets the accessor to operate expendo state for the specified type.
        ///     This is usually used for static territory of non-stataic classes.
        /// </summary>
        /// <typeparam name="T">Target type.</typeparam>
        /// <returns>The expendo state accessor.</returns>
        public virtual IExpendoStateAccessor ForClass<T>() where T : class
        {
            return ForClass(typeof(T));
        }

        /// <summary>
        ///     Gets the accessor to operate expendo state for the specified type.
        ///     This is usually used for static classes.
        /// </summary>
        /// <param name="type">Target type. Must be a class type.</param>
        /// <returns>The expendo state accessor.</returns>
        public IExpendoStateAccessor ForClass(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (!type.IsClass) throw new ArgumentException("Must be a class type", "type");
            return new ExpendoStateAccessor(type, _stateStorage);
        }
    }
}