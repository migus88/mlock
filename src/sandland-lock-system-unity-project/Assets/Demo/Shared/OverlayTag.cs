using System;

namespace Sandland.LockSystem.Demo.Shared
{
    [Serializable, Flags]
    public enum OverlayTags
    {
        None = 0,
        Tint = 1 << 0,  
        Loading = 1 << 1,
        Transparent = 1 << 2
    }
}