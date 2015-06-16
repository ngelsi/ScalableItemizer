namespace ScalableItemizer.Intf
{
    public interface ISuppliedItemizer : IScalableItemizer
    {
        void Supply(double suppliedItems, bool append = false);
    }
}