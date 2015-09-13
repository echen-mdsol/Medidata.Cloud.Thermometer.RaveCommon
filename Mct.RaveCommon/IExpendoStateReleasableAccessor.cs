namespace Medidata.Cloud.Thermometer.RaveCommon
{
    /// <summary>
    ///     Expendo state accessor for instance.
    /// </summary>
    public interface IExpendoStateReleasableAccessor : IExpendoStateAccessor
    {
        /// <summary>
        ///     Release from Expendo state service.
        ///     Usually this should be called in object Finalized method during garbage collected.
        /// </summary>
        void Release();
    }
}