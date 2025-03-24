using System;
using Sandland.LockSystem.Interfaces;

namespace Sandland.LockSystem.Utils
{
    public static class LockServiceUtils
    {
        public static ILock<TLockTags> LockAll<TLockTags>(this ILockService<TLockTags> lockService) where TLockTags : Enum
        {
            return new SimpleLock<TLockTags>(lockService);
        }

        public static ILock<TLockTags> LockAllExcept<TLockTags>(this ILockService<TLockTags> lockService,
            TLockTags excludeTags) where TLockTags : Enum
        {
            return new SimpleLock<TLockTags>(lockService, default, excludeTags);
        }

        public static ILock<TLockTags> LockOnly<TLockTags>(this ILockService<TLockTags> lockService,
            TLockTags includeTags) where TLockTags : Enum
        {
            return new SimpleLock<TLockTags>(lockService, includeTags);
        }
    }
}