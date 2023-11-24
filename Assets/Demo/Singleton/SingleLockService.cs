using Sandland.LockSystem.Demo.Shared;

namespace Sandland.LockSystem.Demo.Singleton
{
    public class SingleLockService : SimpleLockService<LockTag>
    {
        public static SingleLockService Instance { get; } = new();
        
        private SingleLockService()
        {
            
        }
    }
}