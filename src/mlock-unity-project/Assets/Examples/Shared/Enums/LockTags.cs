using System;

namespace Migs.MLock.Examples.Shared.Enums
{
    [Flags]
    public enum LockTags
    {
        None = 0,
        Paused = 1 << 0,
        InCutscene = 1 << 1,
        InDialogue = 1 << 2,
        InTutorial = 1 << 3,
        InCombat = 1 << 4,
        InMenu = 1 << 5,
        Stunned = 1 << 6,
        Dead = 1 << 7,
        InventoryFull = 1 << 8
    }
}