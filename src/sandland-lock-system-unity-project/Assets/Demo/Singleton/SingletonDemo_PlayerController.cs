using Sandland.LockSystem.Demo.Shared;
using Sandland.LockSystem.Interfaces;

namespace Sandland.LockSystem.Demo.Singleton
{
    public class SingletonDemo_PlayerController : PlayerController
    {
        protected override ILockService<InputLockTags> Service => SingletonDemo_InputLockService.Instance;
    }
}