using System.Collections.Generic;

namespace Medidata.Cloud.Thermometer.RaveCommon
{
    /// <summary>
    ///     Expendo state storage
    /// </summary>
    public interface IExpendoStateStorage
    {
        /// <summary>
        ///     Gets a expendo state storage for the specified identity.
        /// </summary>
        /// <param name="targetIdentity">The identity of instance of class.</param>
        /// <returns>The expendo state storage.</returns>
        IDictionary<string, object> GetStorage(int targetIdentity);

        /// <summary>
        ///     Remove the expendo state storage for the specified identity.
        /// </summary>
        /// <param name="targetIdentity">The identity of instance of class.</param>
        void AbandonStorage(int targetIdentity);
    }
}