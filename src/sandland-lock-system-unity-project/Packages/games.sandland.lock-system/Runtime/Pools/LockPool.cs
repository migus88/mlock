using System;
using System.Collections.Generic;
using Sandland.LockSystem.Interfaces;

namespace Sandland.LockSystem.Pools
{
    /// <summary>
    /// Pool for <see cref="ILock{TLockTags}"/> objects to reduce garbage collection pressure
    /// </summary>
    /// <typeparam name="TLockTags">The enum type used for lock tags</typeparam>
    public class LockPool<TLockTags> : ILockPool<TLockTags> where TLockTags : Enum
    {
        public int InitialCapacity { get; }

        private readonly Queue<BaseLock<TLockTags>> _pool;

        /// <summary>
        /// Creates a new pool of locks
        /// </summary>
        /// <param name="initialCapacity">Initial number of locks to pre-allocate</param>
        public LockPool(int initialCapacity = 10)
        {
            if (initialCapacity < 0)
            {
                throw new ArgumentException("Initial capacity cannot be negative", nameof(initialCapacity));
            }

            InitialCapacity = initialCapacity;
            _pool = new Queue<BaseLock<TLockTags>>(initialCapacity);

            // Pre-allocate locks
            for (var i = 0; i < initialCapacity; i++)
            {
                _pool.Enqueue(new BaseLock<TLockTags>());
            }
        }

        /// <inheritdoc />
        public ILock<TLockTags> Borrow(ILockService<TLockTags> lockService, TLockTags includeTags = default,
            TLockTags excludeTags = default)
        {
            if (lockService == null)
            {
                throw new ArgumentNullException(nameof(lockService), "Lock service cannot be null");
            }

            var @lock = _pool.Count > 0 ? _pool.Dequeue() : new BaseLock<TLockTags>();
            @lock.Initialize(lockService, includeTags, excludeTags, this);
            return @lock;
        }

        /// <inheritdoc />
        public void Return(ILock<TLockTags> @lock)
        {
            if (@lock is not BaseLock<TLockTags> baseLock)
            {
                return;
            }

            _pool.Enqueue(baseLock);
        }
    }
}