using ScalableItemizer.Mod;
using System;

namespace ScalableItemizer.Intf
{
    public interface IScalableItemizer : IItemizerBase
    {
        IItemizerItem Add(double itemsPerIteration, Action action);

        IItemizerItem Add(double itemsPerIteration, ItemizerOptions options, Action action);

        void Remove(string identifier);

        void Remove(IItemizerItem item);

        void Start();

        void Stop();
    }
}