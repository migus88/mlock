using System;
using Sandland.LockSystem.Interfaces;

namespace Sandland.LockSystem.Utils
{
    public static class LockServiceUtils
    {
        public static ILock<TLockTag> LockAll<TLockTag>(this ILockService<TLockTag> lockService) where TLockTag : Enum
        {
            return new SimpleLock<TLockTag>(lockService);
        }

        public static ILock<TLockTag> LockAllExcept<TLockTag>(this ILockService<TLockTag> lockService,
            params TLockTag[] excludeTags) where TLockTag : Enum
        {
            return new SimpleLock<TLockTag>(lockService, null, excludeTags);
        }

        public static ILock<TLockTag> LockOnly<TLockTag>(this ILockService<TLockTag> lockService,
            params TLockTag[] includeTags) where TLockTag : Enum
        {
            return new SimpleLock<TLockTag>(lockService, includeTags);
        }
    }
}