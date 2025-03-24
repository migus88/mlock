using Migs.MLock.Interfaces;

namespace Migs.MLock.Tests.Integration
{
    public class GameFeatureController : ILockable<GameFeature>
    {
        public GameFeature LockTags { get; }
        public bool IsLocked { get; private set; }

        public GameFeatureController(GameFeature tags)
        {
            LockTags = tags;
        }

        public void HandleLocking()
        {
            IsLocked = true;
        }

        public void HandleUnlocking()
        {
            IsLocked = false;
        }
    }
}