using ScalableItemizer.Mod;

namespace ScalableItemizer.Intf
{
    public interface IItemizerItem : IItemizerBase
    {
        /// <summary>
        /// The iteration number for this bound action.
        /// </summary>
        double ItemsPerIteration { get; }

        /// <summary>
        /// A unique identifier for this bound action.
        /// </summary>
        string Identifier { get; }

        /// <summary>
        /// The ItemizerOptions flags passed with the bound action.
        /// </summary>
        ItemizerOptions Options { get; }
    }
}