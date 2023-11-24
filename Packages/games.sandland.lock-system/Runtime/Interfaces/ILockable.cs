using System.Collections.Generic;

namespace Sandland.LockSystem.Interfaces
{
    public interface ILockable
    {
        string Category { get; }
        List<ILock> Locks { get; }

        void Lock();
        void Unlock();
    }
}