using System;

namespace ScalableItemizer.Intf
{
    public interface IItemizerBase : IDisposable
    {
        /// <summary>
        /// This event is fired if the worker thread of the action or itemizer has been started.
        /// </summary>
        event EventHandler Started;

        /// <summary>
        /// This event is fired if the worker thread of the action or itemizer has been stopped.
        /// </summary>
        event EventHandler Stopped;

        /// <summary>
        /// This event is fired if the bound action is executed, or the worker thread of the itemizer has started another interval.
        /// </summary>
        event EventHandler Executing;

        /// <summary>
        /// This boolean value specifies whether the worker thread of the action or the itemizer is running.
        /// </summary>
        bool Running { get; }
    }
}