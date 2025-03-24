using System;

namespace Sandland.LockSystem.Interfaces
{
    /// <summary>
    /// Interface for a pool of lockable data 
    /// </summary>
    /// <typeparam name="TLockTags">The enum type used for lock tags</typeparam>
    internal interface ILockableDataPool<TLockTags> where TLockTags : Enum
    {
        /// <summary>
        /// Gets the lockable data from the pool or creates a new one if none are available
        /// </summary>
        /// <returns>A lockable data instance</returns>
        ILockableData<TLockTags> Borrow(ILockable<TLockTags> lockable);
        
        /// <summary>
        /// Returns the lockable data to the pool
        /// </summary>
        /// <param name="data">The lockable data to return to the pool</param>
        void Return(ILockableData<TLockTags> data);
    }
}