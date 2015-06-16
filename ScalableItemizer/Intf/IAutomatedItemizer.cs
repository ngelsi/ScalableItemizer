namespace ScalableItemizer.Intf
{
    public interface IAutomatedItemizer : IScalableItemizer
    {
        double Interval { get; }

        double ItemsPerInterval { get; }
    }
}