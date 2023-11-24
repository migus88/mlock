using System.Collections.Generic;

namespace Sandland.LockSystem.Interfaces
{
    public interface ILockService
    {
        IReadOnlyCollection<ILockable> Lockables { get; }
        
        void AddLockable(ILockable lockable);
        void RemoveLockable(ILockable lockable, bool shouldUnlock);
        
        void AddLock(ILock @lock);
        void RemoveLock(ILock @lock);
    }
}