using System;

namespace ScalableItemizer.Mod
{
    [Flags]
    public enum ItemizerOptions
    {
        Default = 1,
        InheritItems = 2,
        SeparateThread = 4,
    }
}