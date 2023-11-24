using System;

namespace Sandland.LockSystem.Interfaces
{
    public interface ILock : IDisposable
    {
        string Id { get; }
        string Category { get; }
        ILockService LockService { get; }

        void Lock();
        void Unlock();
    }
}