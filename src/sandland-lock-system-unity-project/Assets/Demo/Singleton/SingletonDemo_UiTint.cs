using System;
using Sandland.LockSystem.Demo.Shared;
using Sandland.LockSystem.Lockables;

namespace Sandland.LockSystem.Demo.Singleton
{
    public class SingletonDemo_UiTint : ForegroundUiBlocker<OverlayTag>
    {
        protected override void Awake()
        {
            SingletonDemo_UiOverlayService.Instance.AddLockable(this);
        }

        private void OnDestroy()
        {
            SingletonDemo_UiOverlayService.Instance.RemoveLockable(this, true);
        }
    }
}