using System;
using System.Collections.Generic;

namespace Sandland.LockSystem.Interfaces
{
    public interface ILockService { }

    public interface ILockService<TLockTags> : ILockService where TLockTags : Enum
    {
        IReadOnlyCollection<ILockable<TLockTags>> Lockables { get; }
        
        void AddLockable(ILockable<TLockTags> lockable);
        void RemoveLockable(ILockable<TLockTags> lockable, bool shouldUnlock);
        
        void AddLock(ILock<TLockTags> @lock);
        void RemoveLock(ILock<TLockTags> @lock);
    }
}