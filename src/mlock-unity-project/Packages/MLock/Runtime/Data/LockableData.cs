using System;
using System.Collections.Generic;
using Migs.MLock.Interfaces;

namespace Migs.MLock.Data
{
    internal class LockableData<TLockTags> : ILockableData<TLockTags> where TLockTags : Enum
    {
        public ILockable<TLockTags> Lockable { get; private set; }
        public HashSet<ILock<TLockTags>> Locks { get; } = new();
        public bool IsLocked => Locks.Count > 0;

        public void Init(ILockable<TLockTags> lockable)
        {
            Lockable = lockable;
            Locks.Clear();
        }

        public void Reset()
        {
            Lockable = null;
            Locks.Clear();
        }
    }
}