using System;

namespace Sandland.LockSystem.Interfaces
{
    public interface ILock<TLockTag> : IDisposable where TLockTag : Enum
    {
        string Id { get; }
        TLockTag[] IncludeTags { get; }
        TLockTag[] ExcludeTags { get; }
        ILockService<TLockTag> LockService { get; }

        void Lock();
        void Unlock();
    }
}