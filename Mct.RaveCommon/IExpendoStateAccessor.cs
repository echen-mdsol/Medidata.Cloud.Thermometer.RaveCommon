using System.Collections.Generic;

namespace Medidata.Cloud.Thermometer.RaveCommon
{
    /// <summary>
    ///     Expendo state accessor.
    /// </summary>
    public interface IExpendoStateAccessor
    {
        /// <summary>
        ///     Gets all state keys.
        /// </summary>
        IEnumerable<string> Keys { get; }

        /// <summary>
        ///     Sets a state.
        /// </summary>
        /// <param name="name">State name.</param>
        /// <param name="value">State value.</param>
        /// <returns>The underlying accessor, useful for chaining.</returns>
        IExpendoStateAccessor Set(string name, object value);

        /// <summary>
        ///     Gets a state value.
        /// </summary>
        /// <param name="name">State name.</param>
        /// <returns>The state value.</returns>
        object Get(string name);

        /// <summary>
        ///     Removes a state.
        /// </summary>
        /// <param name="name">State name.</param>
        /// <returns>The underlying accessor, useful for chaining.</returns>
        IExpendoStateAccessor Remove(string name);

        /// <summary>
        ///     Removes all states.
        /// </summary>
        /// <returns>The underlying accessor, useful for chaining.</returns>
        IExpendoStateAccessor RemoveAll();

        /// <summary>
        ///     Checks if state already exists.
        /// </summary>
        /// <param name="name">State name.</param>
        /// <returns>If exists or not.</returns>
        bool Exists(string name);
    }
}