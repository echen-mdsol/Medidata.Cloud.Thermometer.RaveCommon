namespace Medidata.Cloud.Thermometer.RaveCommon
{
    /// <summary>
    ///     Expendo state accessor for instance.
    /// </summary>
    public interface IExpendoStateAbandonableAccessor : IExpendoStateAccessor
    {
        /// <summary>
        ///     Abandon expendo state service.
        ///     Usually this should be called in object Finalized method during garbage collected.
        /// </summary>
        void Abandon();
    }
}