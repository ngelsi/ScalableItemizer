using System;

namespace ScalableItemizer.Mod
{
    /// <summary>
    /// This enum gives options to modify the behaviour of the action bound to the Itemizer.
    /// </summary>
    [Flags]
    public enum ItemizerOptions
    {
        /// <summary>
        /// The default action behaviour.
        /// </summary>
        Default = 1,

        /// <summary>
        /// If this flag is set, the slots passed to the action will be added to the existing number of slots 
        /// instead of overwriting the existing slots.
        /// </summary>
        InheritItems = 2,

        /// <summary>
        /// If this flag is set, the action executions will happen in a separate thread.
        /// </summary>
        SeparateThread = 4,
    }
}