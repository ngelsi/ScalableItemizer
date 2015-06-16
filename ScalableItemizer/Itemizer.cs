using ScalableItemizer.Impl.Itemizers;
using ScalableItemizer.Intf;

namespace ScalableItemizer
{
    public static class Itemizer
    {
        public static IScalableItemizer Automated(double interval, double itemsPerInterval)
        {
            return new AutomatedItemizer(interval, itemsPerInterval);
        }

        public static ISuppliedItemizer Supplied()
        {
            return new SuppliedItemizer();
        }
    }
}