using System;

namespace Medidata.Cloud.Thermometer.RaveCommon
{
    /// <summary>
    ///     Expendo state service
    /// </summary>
    public interface IExpendoStateService
    {
        /// <summary>
        ///     Gets the accessor to operate expendo state for the specified instance.
        /// </summary>
        /// <param name="instance">Object instance.</param>
        /// <returns>The expendo state accessor.</returns>
        IExpendoStateReleasableAccessor ForInstance(object instance);

        /// <summary>
        ///     Gets the accessor to operate expendo state for the specified type.
        ///     This is usually used for static territory of non-stataic classes.
        /// </summary>
        /// <typeparam name="T">Target type.</typeparam>
        /// <returns>The expendo state accessor.</returns>
        IExpendoStateAccessor ForClass<T>() where T : class;

        /// <summary>
        ///     Gets the accessor to operate expendo state for the specified type.
        ///     This is usually used for static classes.
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <returns>The expendo state accessor.</returns>
        IExpendoStateAccessor ForClass(Type type);
    }
}