using System;

namespace Migs.MLock.Tests.Integration
{
    
    [Flags]
    public enum TestLockTags
    {
        None = 0,
        Tag1 = 1 << 0,
        Tag2 = 1 << 1,
        Tag3 = 1 << 2,
        AllTags = Tag1 | Tag2 | Tag3
    }
}