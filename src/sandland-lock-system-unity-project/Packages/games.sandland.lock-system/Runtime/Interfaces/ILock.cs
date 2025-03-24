using System;

namespace Sandland.LockSystem.Interfaces
{
    public interface ILock<TLockTags> : IDisposable where TLockTags : Enum
    {
        string Id { get; }
        TLockTags IncludeTags { get; }
        TLockTags ExcludeTags { get; }
        ILockService<TLockTags> LockService { get; }

        void Lock();
        void Unlock();
    }
}