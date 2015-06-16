using ScalableItemizer.Intf;

namespace ScalableItemizer.Impl.Itemizers
{
    internal class SuppliedItemizer : ScalableItemizer, ISuppliedItemizer
    {
        public SuppliedItemizer() :
            base()
        {
        }

        public void Supply(double suppliedItems, bool append = false)
        {
            lock (lockObject)
            {
                items = append ? items + suppliedItems : suppliedItems;
            }

            ResetEvent.Set();
        }
    }
}