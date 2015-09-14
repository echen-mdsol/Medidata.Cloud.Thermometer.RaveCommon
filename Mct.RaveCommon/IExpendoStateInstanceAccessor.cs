namespace Medidata.Cloud.Thermometer.RaveCommon
{
    /// <summary>
    ///     Expendo state accessor for instance.
    /// </summary>
    public interface IExpendoStateInstanceAccessor : IExpendoStateAccessor
    {
        /// <summary>
        ///     Abandon expendo state service.
        ///     Usually this should be called in object Finalized method during garbage collected.
        /// </summary>
        void Abandon();

        /// <summary>
        ///     Gets accessor for the instance static asset.
        /// </summary>
        IExpendoStateAccessor Static { get; }
    }
}