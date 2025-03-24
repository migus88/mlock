using Sandland.LockSystem.Demo.Shared;
using Sandland.LockSystem.Interfaces;

namespace Sandland.LockSystem.Demo.Singleton
{
    public class SingletonDemo_LockPanel : LockPanel
    {
        protected override ILockService<InputLockTags> Service => SingletonDemo_InputLockService.Instance;
    }
}