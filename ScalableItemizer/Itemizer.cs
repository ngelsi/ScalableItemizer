using ScalableItemizer.Impl.Itemizers;
using ScalableItemizer.Intf;

namespace ScalableItemizer
{
    public static class Itemizer
    {
        /// <summary>
        /// Creates an automated Itemizer which creates the specific number of slots at the specified interval.
        /// </summary>
        /// <param name="interval">The interval in milliseconds at which the slots are generated.</param>
        /// <param name="itemsPerInterval">The number of slots generated at the specified interval.</param>
        /// <returns>The created automated Itemizer.</returns>
        public static IScalableItemizer Automated(double interval, double itemsPerInterval)
        {
            return new AutomatedItemizer(interval, itemsPerInterval);
        }

        /// <summary>
        /// Creates a supplied Itemizer which must be supplied with slots using the Supply() method.
        /// </summary>
        /// <returns>The created supplied Itemizer.</returns>
        public static ISuppliedItemizer Supplied()
        {
            return new SuppliedItemizer();
        }
    }
}