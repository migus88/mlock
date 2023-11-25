using System;
using System.Collections.Generic;

namespace Sandland.LockSystem.Interfaces
{
    public interface ILockable<TLockTag> where TLockTag : Enum
    {
        TLockTag LockTag { get; }
        List<ILock<TLockTag>> Locks { get; }

        void Lock();
        void Unlock();
    }
}