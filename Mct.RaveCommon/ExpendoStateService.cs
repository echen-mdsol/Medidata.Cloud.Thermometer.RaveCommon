using System;
using Medidata.Cloud.Thermometer.RaveCommon.ExpendoState;

namespace Medidata.Cloud.Thermometer.RaveCommon
{
    /// <summary>
    ///     Expendo state service.
    /// </summary>
    public class ExpendoStateService : IExpendoStateService
    {
        private readonly IExpendoStateAccessorFactory _accessorFactory;
        private readonly IExpendoStateStorage _stateStorage;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="stateStorage">An implementation of state storage.</param>
        public ExpendoStateService(IExpendoStateStorage stateStorage = null)
            : this(stateStorage ?? new ExpendoStateConcurrentStorage(), new ExpendoStateAccessorFactory())
        {
        }

        internal ExpendoStateService(IExpendoStateStorage stateStorage, IExpendoStateAccessorFactory accessorFactory)
        {
            if (accessorFactory == null) throw new ArgumentNullException("accessorFactory");
            _stateStorage = stateStorage ?? new ExpendoStateConcurrentStorage();
            _accessorFactory = accessorFactory;
        }

        /// <summary>
        ///     Gets the accessor to operate expendo state for the specified instance.
        /// </summary>
        /// <param name="instance">Object instance.</param>
        /// <returns>The expendo state accessor.</returns>
        public virtual IExpendoStateInstanceAccessor ForInstance(object instance)
        {
            if (instance == null) throw new ArgumentNullException("instance");
            if (instance is Type || instance is string)
                throw new NotSupportedException(string.Format("Instance of '{0}' isn't supported",
                    instance.GetType().FullName));

            return _accessorFactory.CreateInstanceAccessor(instance, _stateStorage, _accessorFactory);
        }

        /// <summary>
        ///     Gets the accessor to operate expendo state for the specified type.
        ///     This is usually used for static territory of non-stataic classes.
        /// </summary>
        /// <typeparam name="T">owner type.</typeparam>
        /// <returns>The expendo state accessor.</returns>
        public virtual IExpendoStateAccessor ForClass<T>() where T : class
        {
            return ForClass(typeof (T));
        }

        /// <summary>
        ///     Gets the accessor to operate expendo state for the specified type.
        ///     This is usually used for static classes.
        /// </summary>
        /// <param name="type">owner type. Must be a class type.</param>
        /// <returns>The expendo state accessor.</returns>
        public IExpendoStateAccessor ForClass(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (!type.IsClass) throw new ArgumentException("Must be a class type", "type");
            return _accessorFactory.CreateAccessor(type, _stateStorage);
        }
    }
}