namespace ScalableItemizer.Intf
{
    public interface IAutomatedItemizer : IScalableItemizer
    {
        /// <summary>
        /// The interval value in milliseconds at which the automated itemizer starts another interval.
        /// </summary>
        double Interval { get; }

        /// <summary>
        /// This specifies how many slots will be generated at each interval.
        /// </summary>
        double ItemsPerInterval { get; }
    }
}