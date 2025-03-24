using System;
using System.Linq;
using Sandland.LockSystem.Interfaces;

namespace Sandland.LockSystem.Utils
{
    public static class LockUtils
    {
        public static bool IsLocked<TLockTags>(this ILockable<TLockTags> lockable) where TLockTags : Enum =>
            lockable.Locks.Count != 0;

        public static bool TryLocking<TLockTags>(this ILockable<TLockTags> lockable, ILock<TLockTags> @lock)
            where TLockTags : Enum
        {
            if (!@lock.ShouldLock(lockable))
            {
                return false;
            }

            lockable.Locks.Add(@lock);
            lockable.UpdateLockState();
            return true;
        }

        public static bool TryUnlocking<TLockTags>(this ILockable<TLockTags> lockable, ILock<TLockTags> @lock)
            where TLockTags : Enum
        {
            if (!lockable.Locks.Contains(@lock))
            {
                return false;
            }

            lockable.Locks.Remove(@lock);
            lockable.UpdateLockState();
            return lockable.IsLocked();
        }

        public static void UpdateLockState<TLockTags>(this ILockable<TLockTags> lockable) where TLockTags : Enum
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

        private static bool ShouldLock<TLockTags>(this ILock<TLockTags> @lock, ILockable<TLockTags> lockable)
            where TLockTags : Enum
        {
            if (lockable.Locks.Contains(@lock))
            {
                return false;
            }

            // Get integer values for all tags
            int includeTags = Convert.ToInt32(@lock.IncludeTags);
            int excludeTags = Convert.ToInt32(@lock.ExcludeTags);
            int lockableTags = Convert.ToInt32(lockable.LockTags);

            // If both include and exclude are 0 (default), lock everything
            if (includeTags == 0 && excludeTags == 0)
            {
                return true;
            }

            // If both include and exclude have values, that's an invalid configuration
            if (includeTags != 0 && excludeTags != 0)
            {
                return false;
            }

            // Check if any lockable tag is included in the include tags
            if (includeTags != 0)
            {
                return (includeTags & lockableTags) != 0;
            }

            // Check if no lockable tag is in the exclude tags
            if (excludeTags != 0)
            {
                return (excludeTags & lockableTags) == 0;
            }

            return false;
        }
    }
}