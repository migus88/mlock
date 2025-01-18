using Sandland.LockSystem.Demo.Shared;

namespace Sandland.LockSystem.Demo.Singleton
{
    public class SingletonDemo_InputLockService : SimpleLockService<InputLockTag>
    {
        public static SingletonDemo_InputLockService Instance { get; } = new();
        
        private SingletonDemo_InputLockService()
        {
            
        }
    }
}