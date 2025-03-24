using System;
using Sandland.LockSystem.Interfaces;

namespace Sandland.LockSystem
{
    /// <summary>
    /// A base implementation of <see cref="ILock{TLockTags}"/> with built-in pooling support
    /// </summary>
    /// <typeparam name="TLockTags">The enum type used for lock tags</typeparam>
    internal class BaseLock<TLockTags> : ILock<TLockTags> where TLockTags : Enum
    {
        // Static counter for ID generation - much more efficient than GUIDs
        private static int _idCounter;
        
        /// <summary>
        /// Whether the lock is currently active
        /// </summary>
        public bool IsLocked { get; protected set; }
        
        /// <summary>
        /// Unique identifier for this lock
        /// </summary>
        public virtual int Id { get; protected set; }
        
        /// <summary>
        /// Tags that this lock applies to (if any)
        /// </summary>
        public TLockTags IncludeTags { get; protected set; }
        
        /// <summary>
        /// Tags that this lock does not apply to (if any)
        /// </summary>
        public TLockTags ExcludeTags { get; protected set; }
        
        /// <summary>
        /// The lock service that manages this lock
        /// </summary>
        public ILockService<TLockTags> LockService { get; protected set; }

        // Reference to the pool that created this lock, for returning on disposal
        private ILockPool<TLockTags> _pool;
        
        /// <summary>
        /// Creates a new empty lock for pooling
        /// </summary>
        public BaseLock()
        {
        }

        /// <summary>
        /// Creates a new lock
        /// </summary>
        /// <param name="lockService">The lock service to associate with the lock</param>
        /// <param name="includeTags">Tags to include for locking</param>
        /// <param name="excludeTags">Tags to exclude from locking</param>
        /// <param name="shouldLockImmediately">Whether to lock immediately</param>
        /// <param name="pool">Optional pool to return to on disposal</param>
        /// <exception cref="ArgumentNullException">Thrown if lockService is null</exception>
        public BaseLock(ILockService<TLockTags> lockService, TLockTags includeTags = default, TLockTags excludeTags = default, ILockPool<TLockTags> pool = null)
        {
            Initialize(lockService, includeTags, excludeTags, pool);
        }
        
        /// <summary>
        /// Initializes or reinitializes the lock with new parameters
        /// </summary>
        /// <param name="lockService">The lock service to associate with the lock</param>
        /// <param name="includeTags">Tags to include for locking</param>
        /// <param name="excludeTags">Tags to exclude from locking</param>
        /// <param name="shouldLockImmediately">Whether to lock immediately</param>
        /// <param name="pool">Optional pool to return to on disposal</param>
        public void Initialize(ILockService<TLockTags> lockService, TLockTags includeTags = default, TLockTags excludeTags = default, ILockPool<TLockTags> pool = null)
        {
            // If already locked, unlock first
            if (IsLocked)
            {
                Unlock();
            }
            
            _pool = pool;
            // Use an atomic increment operation for thread safety
            Id = System.Threading.Interlocked.Increment(ref _idCounter);
            SetService(lockService);
            SetIncludeTags(includeTags);
            SetExcludeTags(excludeTags);
        }

        /// <summary>
        /// Sets the lock service
        /// </summary>
        /// <param name="lockService">The lock service</param>
        /// <exception cref="ArgumentNullException">Thrown if lockService is null</exception>
        public void SetService(ILockService<TLockTags> lockService)
        {
            LockService = lockService ?? throw new ArgumentNullException(nameof(lockService), "Lock service cannot be null");
        }

        /// <summary>
        /// Sets the include tags
        /// </summary>
        /// <param name="includeTags">Tags to include</param>
        public void SetIncludeTags(TLockTags includeTags)
        {
            IncludeTags = includeTags;
        }

        /// <summary>
        /// Sets the exclude tags
        /// </summary>
        /// <param name="excludeTags">Tags to exclude</param>
        public void SetExcludeTags(TLockTags excludeTags)
        {
            ExcludeTags = excludeTags;
        }

        /// <inheritdoc />
        public void Lock()
        {
            if (IsLocked)
            {
                return;
            }
            
            LockService.TryLocking(this);
            IsLocked = true;
        }

        /// <inheritdoc />
        public void Unlock()
        {
            if (!IsLocked)
            {
                return;
            }

            LockService.TryUnlocking(this);
            IsLocked = false;
        }

        /// <inheritdoc />
        public override string ToString() => $"{nameof(BaseLock<TLockTags>)}||{Id}";

        /// <summary>
        /// Unlocks and returns to pool if this lock is pooled
        /// </summary>
        public virtual void Dispose()
        {
            Unlock();
            
            // Return to pool if we have one
            if (_pool != null)
            {
                _pool.Return(this);
            }
        }
    }
} 