using System;

namespace Migs.Examples.Shared
{
    /// <summary>
    /// Tags used for locking different parts of the car system
    /// </summary>
    [Flags, Serializable]
    public enum CarLockTags
    {
        None = 0,
        PlayerInput = 1 << 0,
        GameHud = 1 << 1,
        MainMenu = 1 << 2
    }
}