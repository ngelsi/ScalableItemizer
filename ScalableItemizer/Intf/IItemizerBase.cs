using System;

namespace ScalableItemizer.Intf
{
    public interface IItemizerBase : IDisposable
    {
        event EventHandler Started;

        event EventHandler Stopped;

        event EventHandler Executing;

        bool Running { get; }
    }
}