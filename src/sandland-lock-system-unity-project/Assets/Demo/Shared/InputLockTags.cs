using System;

namespace Sandland.LockSystem.Demo.Shared
{
    [Serializable, Flags]
    public enum InputLockTags
    {
        Player1 = 1,
        Player2 = 2,
        UI = 4,
    }
}