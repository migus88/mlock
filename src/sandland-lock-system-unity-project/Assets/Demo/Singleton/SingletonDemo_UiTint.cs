using System;
using Sandland.LockSystem.Attributes;
using Sandland.LockSystem.Demo.Shared;
using Sandland.LockSystem.Lockables;

namespace Sandland.LockSystem.Demo.Singleton
{
    [LockableBehaviour("SingletonDemo_UiOverlayService.Instance")]
    public class SingletonDemo_UiTint : ForegroundUiBlocker<OverlayTags>
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