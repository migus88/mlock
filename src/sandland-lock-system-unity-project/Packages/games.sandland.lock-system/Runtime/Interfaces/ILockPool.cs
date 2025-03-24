using System;

namespace Sandland.LockSystem.Interfaces
{
    /// <summary>
    /// Interface for a pool of lock objects to reduce allocations
    /// </summary>
    /// <typeparam name="TLockTags">The enum type used for lock tags</typeparam>
    public interface ILockPool<TLockTags> where TLockTags : Enum
    {
        int InitialCapacity { get; }
        
        /// <summary>
        /// Gets a lock from the pool or creates a new one if none are available
        /// </summary>
        /// <param name="lockService">The lock service to associate with the lock</param>
        /// <param name="includeTags">Tags to include for locking</param>
        /// <param name="excludeTags">Tags to exclude from locking</param>
        /// <returns>A lock instance</returns>
        ILock<TLockTags> Borrow(ILockService<TLockTags> lockService, TLockTags includeTags = default, TLockTags excludeTags = default);
        
        /// <summary>
        /// Returns a lock to the pool
        /// </summary>
        /// <param name="lock">The lock to return to the pool</param>
        void Return(ILock<TLockTags> @lock);
    }
} 