using System;
using Sandland.LockSystem.Interfaces;

namespace Sandland.LockSystem
{
    public class SimpleLock<TLockTags> : ILock<TLockTags> where TLockTags : Enum
    {
        public bool IsLocked { get; private set; }
        public virtual string Id { get; }
        public TLockTags IncludeTags { get; }
        public TLockTags ExcludeTags { get; }
        public ILockService<TLockTags> LockService { get; }

        public SimpleLock(ILockService<TLockTags> lockService, TLockTags includeTags = default, TLockTags excludeTags = default, bool shouldLockImmediately = true)
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

        public override string ToString() => $"{nameof(SimpleLock<TLockTags>)}||{Id}";

        public void Dispose()
        {
            Unlock();
        }
    }
}