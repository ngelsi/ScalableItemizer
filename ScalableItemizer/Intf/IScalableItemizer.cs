using ScalableItemizer.Mod;
using System;

namespace ScalableItemizer.Intf
{
    public interface IScalableItemizer : IItemizerBase
    {
        /// <summary>
        /// This event is fired when an exception occurs during the execution of the worker thread
        /// of the itemizer.
        /// </summary>
        event EventHandler<UnhandledExceptionEventArgs> Exception;

        /// <summary>
        /// Bind an action to the itemizer and set the iteration number for the specified action.
        /// </summary>
        /// <param name="itemsPerIteration">The action will be executed this many times at every elapsed interval.</param>
        /// <param name="action">The action which will be executed by the itemizer.</param>
        /// <returns>The bound action.</returns>
        IItemizerItem Add(double itemsPerIteration, Action<IItemizerItem> action);

        /// <summary>
        /// Bind an action to the itemizer and set the iteration number for the specified action.
        /// </summary>
        /// <param name="itemsPerIteration">The action will be executed this many times at every elapsed interval.</param>
        /// <param name="options">ItemizerOptions Enum flags passed with the action to modify its behaviour.</param>
        /// <param name="action">The action which will be executed by the itemizer.</param>
        /// <returns>The bound action.</returns>
        IItemizerItem Add(double itemsPerIteration, ItemizerOptions options, Action<IItemizerItem> action);

        /// <summary>
        /// Bind an action to the itemizer and set the iteration number for the specified action.
        /// </summary>
        /// <param name="itemsPerIteration">This supplied function must return a double value which will
        /// determine the iteration number for the specified action.</param>
        /// <param name="action">The action which will be executed by the itemizer.</param>
        /// <returns>The bound action.</returns>
        IItemizerItem Add(Func<double> itemsPerIteration, Action<IItemizerItem> action);

        /// <summary>
        /// Bind an action to the itemizer and set the iteration number for the specified action.
        /// </summary>
        /// <param name="itemsPerIteration">This supplied function must return a double value which will
        /// determine the iteration number for the specified action.</param>
        /// <param name="options">ItemizerOptions Enum flags passed with the action to modify its behaviour.</param>
        /// <param name="action">The action which will be executed by the itemizer.</param>
        /// <returns>The bound action.</returns>
        IItemizerItem Add(Func<double> itemsPerIteration, ItemizerOptions options, Action<IItemizerItem> action);

        /// <summary>
        /// Removes a bound action from the itemizer using its string identifier.
        /// </summary>
        /// <param name="identifier">The string identifier of the action.</param>
        void Remove(string identifier);

        /// <summary>
        /// Removes a bound action from the itemizer.
        /// </summary>
        /// <param name="item">The bound action which is to be removed.</param>
        void Remove(IItemizerItem item);

        /// <summary>
        /// Starts the worker thread of the itemizer.
        /// </summary>
        void Start();

        /// <summary>
        /// Halts the worker thread of the itemizer.
        /// </summary>
        void Stop();
    }
}