namespace ScalableItemizer.Intf
{
    public interface ISuppliedItemizer : IScalableItemizer
    {
        /// <summary>
        /// Supply the itemizer with the specified number of slots. These slots will be distributed among the
        /// bound actions according to their iteration number.
        /// </summary>
        /// <param name="suppliedItems">The number of slots to supply the itemizer.</param>
        /// <param name="append">If this is true, the specified slot count will be added to the existing slot count instead of overwriting it.</param>
        void Supply(double suppliedItems, bool append = false);
    }
}