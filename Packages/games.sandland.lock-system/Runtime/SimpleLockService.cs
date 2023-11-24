using System;
using System.Collections.Generic;
using Sandland.LockSystem.Interfaces;
using Sandland.LockSystem.Utils;

namespace Sandland.LockSystem
{
    public class SimpleLockService<TLockTag> : ILockService<TLockTag> where TLockTag : Enum
    {
        public IReadOnlyCollection<ILockable<TLockTag>> Lockables => _lockables;

        private readonly List<ILockable<TLockTag>> _lockables = new();
        
        public void AddLockable(ILockable<TLockTag> lockable)
        {
            if (_lockables.Contains(lockable))
            {
                return;
            }
            
            _lockables.Add(lockable);
        }

        public void RemoveLockable(ILockable<TLockTag> lockable, bool shouldUnlock)
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

        public void AddLock(ILock<TLockTag> @lock)
        {
            foreach (var lockable in _lockables)
            {
                lockable.TryLocking(@lock);
            }
        }

        public void RemoveLock(ILock<TLockTag> @lock)
        {
            foreach (var lockable in _lockables)
            {
                lockable.TryUnlocking(@lock);
            }
        }
    }
}