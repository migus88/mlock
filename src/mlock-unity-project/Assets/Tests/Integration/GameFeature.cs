using System;

namespace Migs.MLock.Tests.Integration
{
    [Flags]
    public enum GameFeature
    {
        None = 0,
        Movement = 1 << 0,
        Inventory = 1 << 1,
        Combat = 1 << 2,
        Dialog = 1 << 3,
        Menu = 1 << 4,
        All = Movement | Inventory | Combat | Dialog | Menu
    }
}