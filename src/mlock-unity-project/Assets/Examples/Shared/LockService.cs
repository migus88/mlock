using Migs.MLock.Examples.Shared.Enums;
using Migs.MLock.Interfaces;
using UnityEngine;

namespace Migs.MLock.Examples.Shared
{
    /// <summary>
    /// A singleton implementation of the lock service.
    /// WARNING: Using singletons is generally considered a bad practice as it:
    /// 1. Makes code harder to test
    /// 2. Creates hidden dependencies
    /// 3. Makes it difficult to swap implementations
    /// 4. Can lead to global state issues
    /// 
    /// Instead, consider using dependency injection (DI) to manage your services.
    /// This is just an example for demonstration purposes.
    /// </summary>
    public class LockService : MonoBehaviour, ILockService<LockTags>
    {
        private static LockService _instance;
        private BaseLockService<LockTags> _lockService;

        public static LockService Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("LockService");
                    _instance = go.AddComponent<LockService>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _lockService = new BaseLockService<LockTags>();
        }

        public void Subscribe(ILockable<LockTags> lockable) => _lockService.Subscribe(lockable);
        public void Unsubscribe(ILockable<LockTags> lockable) => _lockService.Unsubscribe(lockable);
        public void TryLocking(ILock<LockTags> @lock) => _lockService.TryLocking(@lock);
        public void TryUnlocking(ILock<LockTags> @lock) => _lockService.TryUnlocking(@lock);
        public ILock<LockTags> Lock(LockTags includeTags) => _lockService.Lock(includeTags);
        public ILock<LockTags> LockAllExcept(LockTags excludeTags) => _lockService.LockAllExcept(excludeTags);
        public ILock<LockTags> LockAll() => _lockService.LockAll();
        public bool IsLocked(ILockable<LockTags> lockable) => _lockService.IsLocked(lockable);
    }
} 