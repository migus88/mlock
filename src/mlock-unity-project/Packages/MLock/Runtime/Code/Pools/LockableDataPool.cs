using System;
using System.Collections.Generic;
using Migs.MLock.Data;
using Migs.MLock.Interfaces;

namespace Migs.MLock.Pools
{
    internal class LockableDataPool<TLockTags> : ILockableDataPool<TLockTags> where TLockTags : Enum
    {
        private readonly Queue<ILockableData<TLockTags>> _pool;

        /// <summary>
        /// Creates a new pool of lockable data
        /// </summary>
        /// <param name="initialCapacity">Initial number of items to pre-allocate</param>
        public LockableDataPool(int initialCapacity = 10)
        {
            if (initialCapacity < 0)
            {
                throw new ArgumentException("Initial capacity cannot be negative", nameof(initialCapacity));
            }

            _pool = new Queue<ILockableData<TLockTags>>(initialCapacity);

            // Pre-allocate locks
            for (var i = 0; i < initialCapacity; i++)
            {
                _pool.Enqueue(new LockableData<TLockTags>());
            }
        }

        public ILockableData<TLockTags> Borrow(ILockable<TLockTags> lockable)
        {
            if (lockable == null)
            {
                throw new ArgumentNullException(nameof(lockable), "Lockable cannot be null");
            }

            var data = _pool.Count > 0 ? _pool.Dequeue() : new LockableData<TLockTags>();
            data.Init(lockable);
            return data;
        }

        public void Return(ILockableData<TLockTags> data)
        {
            if (data is not LockableData<TLockTags> baseData)
            {
                return;
            }

            data.Reset();
            _pool.Enqueue(baseData);
        }
    }
}