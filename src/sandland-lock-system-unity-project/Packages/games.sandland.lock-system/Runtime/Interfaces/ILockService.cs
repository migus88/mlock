using System;
using System.Collections.Generic;

namespace Sandland.LockSystem.Interfaces
{
    public interface ILockService { }

    public interface ILockService<TLockTag> : ILockService where TLockTag : Enum
    {
        IReadOnlyCollection<ILockable<TLockTag>> Lockables { get; }
        
        void AddLockable(ILockable<TLockTag> lockable);
        void RemoveLockable(ILockable<TLockTag> lockable, bool shouldUnlock);
        
        void AddLock(ILock<TLockTag> @lock);
        void RemoveLock(ILock<TLockTag> @lock);
    }
}