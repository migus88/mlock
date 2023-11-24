using System.Collections.Generic;
using Sandland.LockSystem.Interfaces;
using Sandland.LockSystem.Utils;

namespace Sandland.LockSystem
{
    public class BasicLockService : ILockService
    {
        public IReadOnlyCollection<ILockable> Lockables => _lockables;

        private readonly List<ILockable> _lockables = new();
        
        public void AddLockable(ILockable lockable)
        {
            if (_lockables.Contains(lockable))
            {
                return;
            }
            
            _lockables.Add(lockable);
        }

        public void RemoveLockable(ILockable lockable, bool shouldUnlock)
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

        public void AddLock(ILock @lock)
        {
            foreach (var lockable in _lockables)
            {
                lockable.TryLocking(@lock);
            }
        }

        public void RemoveLock(ILock @lock)
        {
            foreach (var lockable in _lockables)
            {
                lockable.TryUnlocking(@lock);
            }
        }
    }
}