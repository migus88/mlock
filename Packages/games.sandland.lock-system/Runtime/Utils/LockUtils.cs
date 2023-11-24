using Sandland.LockSystem.Interfaces;

namespace Sandland.LockSystem.Utils
{
    public static class LockUtils
    {
        public static bool IsLocked(this ILockable lockable) => lockable.Locks.Count != 0;
        
        public static bool TryLocking(this ILockable lockable, ILock @lock)
        {
            if (@lock.Category != lockable.Category || lockable.Locks.Contains(@lock))
            {
                return false;
            }
            
            lockable.Locks.Add(@lock);
            lockable.UpdateLockState();
            return true;
        }

        public static bool TryUnlocking(this ILockable lockable, ILock @lock)
        {
            if (!lockable.Locks.Contains(@lock))
            {
                return false;
            }

            lockable.Locks.Remove(@lock);
            lockable.UpdateLockState();
            return lockable.IsLocked();
        }

        public static void UpdateLockState(this ILockable lockable)
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
    }
}