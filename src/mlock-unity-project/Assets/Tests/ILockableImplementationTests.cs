using System;
using FluentAssertions;
using Migs.MLock.Interfaces;
using Migs.MLock.Tests.Integration;
using NUnit.Framework;

namespace Migs.MLock.Tests
{
    public class ILockableImplementationTests
    {
        /// <summary>
        /// Test implementation of ILockable that raises events when locked/unlocked
        /// </summary>
        private class TestLockable : ILockable<TestLockTags>
        {
            public event Action OnLocked;
            public event Action OnUnlocked;
            
            public TestLockTags LockTags { get; }
            public bool IsLocked { get; private set; }
            public int LockCount { get; private set; }
            public int UnlockCount { get; private set; }
            
            public TestLockable(TestLockTags tags)
            {
                LockTags = tags;
            }
            
            public void HandleLocking()
            {
                IsLocked = true;
                LockCount++;
                OnLocked?.Invoke();
            }
            
            public void HandleUnlocking()
            {
                IsLocked = false;
                UnlockCount++;
                OnUnlocked?.Invoke();
            }
        }
        
        private BaseLockService<TestLockTags> _lockService;
        
        [SetUp]
        public void Setup()
        {
            _lockService = new BaseLockService<TestLockTags>();
        }
        
        [Test]
        public void TestLockable_RaisesEvents_WhenLockStateChanges()
        {
            // Arrange
            var lockable = new TestLockable(TestLockTags.Tag1);
            var lockEventRaised = false;
            var unlockEventRaised = false;
            
            lockable.OnLocked += () => lockEventRaised = true;
            lockable.OnUnlocked += () => unlockEventRaised = true;
            
            _lockService.Subscribe(lockable);
            
            // Act - Lock
            var lockObj = _lockService.Lock(TestLockTags.Tag1);
            
            // Assert
            lockEventRaised.Should().BeTrue("lock event should be raised");
            unlockEventRaised.Should().BeFalse("unlock event should not be raised yet");
            lockable.IsLocked.Should().BeTrue();
            lockable.LockCount.Should().Be(1);
            lockable.UnlockCount.Should().Be(0);
            
            // Reset and test unlock
            lockEventRaised = false;
            
            // Act - Unlock
            lockObj.Dispose();
            
            // Assert
            lockEventRaised.Should().BeFalse("lock event should not be raised again");
            unlockEventRaised.Should().BeTrue("unlock event should be raised");
            lockable.IsLocked.Should().BeFalse();
            lockable.LockCount.Should().Be(1);
            lockable.UnlockCount.Should().Be(1);
        }
        
        [Test]
        public void TestLockable_HandlesMultipleLocks_CallsHandlersOnceOnly()
        {
            // Arrange
            var lockable = new TestLockable(TestLockTags.Tag1);
            _lockService.Subscribe(lockable);
            
            // Act - Apply two locks
            var lock1 = _lockService.Lock(TestLockTags.Tag1);
            var lock2 = _lockService.Lock(TestLockTags.Tag1);
            
            // Assert
            lockable.IsLocked.Should().BeTrue();
            lockable.LockCount.Should().Be(1, "HandleLocking should be called exactly once regardless of lock count");
            
            // Act - Remove one lock
            lock1.Dispose();
            
            // Assert
            lockable.IsLocked.Should().BeTrue("should still be locked by second lock");
            lockable.UnlockCount.Should().Be(0, "HandleUnlocking should not be called until all locks are removed");
            
            // Act - Remove second lock
            lock2.Dispose();
            
            // Assert
            lockable.IsLocked.Should().BeFalse("should be unlocked when all locks are removed");
            lockable.UnlockCount.Should().Be(1, "HandleUnlocking should be called exactly once when final lock is removed");
        }
        
        [Test]
        public void TestLockable_WithDifferentTags_RespondsCorrectlyToLocks()
        {
            // Arrange
            var lockable1 = new TestLockable(TestLockTags.Tag1);
            var lockable2 = new TestLockable(TestLockTags.Tag2);
            var lockable3 = new TestLockable(TestLockTags.Tag1 | TestLockTags.Tag2);
            var lockable4 = new TestLockable(TestLockTags.Tag3);
            
            _lockService.Subscribe(lockable1);
            _lockService.Subscribe(lockable2);
            _lockService.Subscribe(lockable3);
            _lockService.Subscribe(lockable4);
            
            // Act - Lock Tag1
            using (var lockObj = _lockService.Lock(TestLockTags.Tag1))
            {
                // Assert
                lockable1.IsLocked.Should().BeTrue("Tag1 lockable should be locked");
                lockable2.IsLocked.Should().BeFalse("Tag2 lockable should not be locked");
                lockable3.IsLocked.Should().BeTrue("Tag1|Tag2 lockable should be locked by Tag1");
                lockable4.IsLocked.Should().BeFalse("Tag3 lockable should not be locked");
            }
            
            // All should be unlocked now
            lockable1.IsLocked.Should().BeFalse();
            lockable2.IsLocked.Should().BeFalse();
            lockable3.IsLocked.Should().BeFalse();
            lockable4.IsLocked.Should().BeFalse();
            
            // Act - Lock All Except Tag2
            using (var lockObj = _lockService.LockAllExcept(TestLockTags.Tag2))
            {
                // Assert
                lockable1.IsLocked.Should().BeTrue("Tag1 should be locked");
                lockable2.IsLocked.Should().BeFalse("Tag2 should not be locked (excluded)");
                lockable3.IsLocked.Should().BeFalse("Tag1|Tag2 should not be locked because Tag2 was excluded and we ignored Tag1");
                lockable4.IsLocked.Should().BeTrue("Tag3 should be locked");
            }
        }
        
        [Test]
        public void TestLockable_Unsubscribe_ClearsLockState()
        {
            // Arrange
            var lockable = new TestLockable(TestLockTags.Tag1);
            _lockService.Subscribe(lockable);
            
            // Lock the lockable
            var lockObj = _lockService.Lock(TestLockTags.Tag1);
            lockable.IsLocked.Should().BeTrue();
            
            // Act - Unsubscribe while locked
            _lockService.Unsubscribe(lockable);
            
            // Assert
            lockable.IsLocked.Should().BeFalse("Unsubscribe should call HandleUnlocking");
            lockable.UnlockCount.Should().Be(1);
            
            // Cleanup
            lockObj.Dispose();
        }
    }
} 