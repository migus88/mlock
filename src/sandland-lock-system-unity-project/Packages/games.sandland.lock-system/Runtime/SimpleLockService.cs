using System;
using System.Collections.Generic;
using Sandland.LockSystem.Interfaces;
using Sandland.LockSystem.Utils;

namespace Sandland.LockSystem
{
    public class SimpleLockService<TLockTags> : ILockService<TLockTags> where TLockTags : Enum
    {
        public IReadOnlyCollection<ILockable<TLockTags>> Lockables => _lockables;

        private readonly List<ILockable<TLockTags>> _lockables = new();
        
        public void AddLockable(ILockable<TLockTags> lockable)
        {
            if (_lockables.Contains(lockable))
            {
                return;
            }
            
            _lockables.Add(lockable);
        }

        public void RemoveLockable(ILockable<TLockTags> lockable, bool shouldUnlock)
        {
            if (!_lockables.Contains(lockable))
            {
                return;
            }

            if (shouldUnlock)
            {
                lockable.Locks.Clear();
                lockable.Unlock();
            }

            _lockables.Remove(lockable);
        }

        public void AddLock(ILock<TLockTags> @lock)
        {
            foreach (var lockable in _lockables)
            {
                lockable.TryLocking(@lock);
            }
        }

        public void RemoveLock(ILock<TLockTags> @lock)
        {
            foreach (var lockable in _lockables)
            {
                lockable.TryUnlocking(@lock);
            }
        }
    }
}