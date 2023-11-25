using System;
using System.Linq;
using Sandland.LockSystem.Interfaces;

namespace Sandland.LockSystem.Utils
{
    public static class LockUtils
    {
        public static bool IsLocked<TLockTag>(this ILockable<TLockTag> lockable) where TLockTag : Enum =>
            lockable.Locks.Count != 0;

        public static bool TryLocking<TLockTag>(this ILockable<TLockTag> lockable, ILock<TLockTag> @lock)
            where TLockTag : Enum
        {
            if (!@lock.ShouldLock(lockable))
            {
                return false;
            }

            lockable.Locks.Add(@lock);
            lockable.UpdateLockState();
            return true;
        }

        public static bool TryUnlocking<TLockTag>(this ILockable<TLockTag> lockable, ILock<TLockTag> @lock)
            where TLockTag : Enum
        {
            if (!lockable.Locks.Contains(@lock))
            {
                return false;
            }

            lockable.Locks.Remove(@lock);
            lockable.UpdateLockState();
            return lockable.IsLocked();
        }

        public static void UpdateLockState<TLockTag>(this ILockable<TLockTag> lockable) where TLockTag : Enum
        {
            if (lockable.Locks.Count == 0)
            {
                lockable.Unlock();
            }
            else
            {
                lockable.Lock();
            }
        }

        private static bool ShouldLock<TLockTag>(this ILock<TLockTag> @lock, ILockable<TLockTag> lockable)
            where TLockTag : Enum
        {
            if (lockable.Locks.Contains(@lock))
            {
                return false;
            }

            var isIncludeListEmpty = @lock.IncludeTags == null || @lock.IncludeTags.Length == 0;
            var isExcludeListEmpty = @lock.ExcludeTags == null || @lock.ExcludeTags.Length == 0;

            // When both of the lists are empty, this means that we're locking everything
            if (isIncludeListEmpty && isExcludeListEmpty)
            {
                return true;
            }

            if (!isIncludeListEmpty && !isExcludeListEmpty)
            {
                return false;
            }

            if (!isIncludeListEmpty && @lock.IncludeTags.Contains(lockable.LockTag))
            {
                return true;
            }

            return !isExcludeListEmpty && !@lock.ExcludeTags.Contains(lockable.LockTag);
        }
    }
}