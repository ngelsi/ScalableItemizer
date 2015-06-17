using ScalableItemizer.Mod;
using System;

namespace ScalableItemizer.Intf
{
    public interface IScalableItemizer : IItemizerBase
    {
        event EventHandler<UnhandledExceptionEventArgs> Exception;

        IItemizerItem Add(double itemsPerIteration, Action<IItemizerItem> action);

        IItemizerItem Add(double itemsPerIteration, ItemizerOptions options, Action<IItemizerItem> action);

        IItemizerItem Add(Func<double> itemsPerIteration, Action<IItemizerItem> action);

        IItemizerItem Add(Func<double> itemsPerIteration, ItemizerOptions options, Action<IItemizerItem> action);

        void Remove(string identifier);

        void Remove(IItemizerItem item);

        void Start();

        void Stop();
    }
}