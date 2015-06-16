using ScalableItemizer.Mod;

namespace ScalableItemizer.Intf
{
    public interface IItemizerItem : IItemizerBase
    {
        double ItemsPerIteration { get; }

        string Identifier { get; }

        ItemizerOptions Options { get; }
    }
}