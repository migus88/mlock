using Sandland.LockSystem.Interfaces;
using Sandland.LockSystem.Lockables;

namespace Sandland.LockSystem.Demo
{
    internal static class LockFactory
    {
        public static ILock TintLock(this ILockService service)
        {
            return new LockServiceLock(nameof(ForegroundTintLockable), service);
        }
    }
}