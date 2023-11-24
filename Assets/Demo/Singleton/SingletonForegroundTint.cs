using System;
using Sandland.LockSystem.Demo.Shared;
using Sandland.LockSystem.Lockables;

namespace Sandland.LockSystem.Demo.Singleton
{
    public class SingletonForegroundTint : ForegroundTintLockable<LockTag>
    {
        private void Awake()
        {
            SingleLockService.Instance.AddLockable(this);
        }

        private void OnDestroy()
        {
            SingleLockService.Instance.RemoveLockable(this, true);
        }
    }
}