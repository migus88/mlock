using System;
using Sandland.LockSystem.Interfaces;

namespace Sandland.LockSystem
{
    public class SimpleLock<TLockTag> : ILock<TLockTag> where TLockTag : Enum
    {
        public bool IsLocked { get; private set; }
        public virtual string Id { get; }
        public TLockTag[] IncludeTags { get; }
        public TLockTag[] ExcludeTags { get; }
        public ILockService<TLockTag> LockService { get; }

        public SimpleLock(ILockService<TLockTag> lockService, TLockTag[] includeTags = null, TLockTag[] excludeTags = null, bool shouldLockImmediately = true)
        {
            Id = Guid.NewGuid().ToString();
            LockService = lockService;
            IncludeTags = includeTags;
            ExcludeTags = excludeTags;

            if (LockService == null)
            {
                throw new Exception($"{nameof(lockService)} cannot be null");
            }

            if (shouldLockImmediately)
            {
                Lock();
            }
        }

        public void Lock()
        {
            if (IsLocked)
            {
                return;
            }
            
            LockService.AddLock(this);
            IsLocked = true;
        }

        public void Unlock()
        {
            if (!IsLocked)
            {
                return;
            }

            LockService.RemoveLock(this);
            IsLocked = false;
        }

        public override string ToString() => $"{nameof(SimpleLock<TLockTag>)}||{Id}";

        public void Dispose()
        {
            Unlock();
        }
    }
}