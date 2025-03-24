using System;
using System.Collections.Generic;

namespace Sandland.LockSystem.Interfaces
{
    public interface ILockable<TLockTags> where TLockTags : Enum
    {
        TLockTags LockTags { get; }
        List<ILock<TLockTags>> Locks { get; }

        void Lock();
        void Unlock();
    }
}