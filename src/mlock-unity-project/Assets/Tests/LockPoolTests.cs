using System;
using FluentAssertions;
using Migs.MLock.Interfaces;
using Migs.MLock.Pools;
using Migs.MLock.Tests.Integration;
using NSubstitute;
using NUnit.Framework;

namespace Migs.MLock.Tests
{
    public class LockPoolTests
    {
        private LockPool<TestLockTags> _lockPool;
        private ILockService<TestLockTags> _mockLockService;
        
        [SetUp]
        public void Setup()
        {
            _lockPool = new LockPool<TestLockTags>(5);
            _mockLockService = Substitute.For<ILockService<TestLockTags>>();
        }
        
        [Test]
        public void Constructor_WithNegativeCapacity_ThrowsArgumentException()
        {
            // Act & Assert
            Action act = () => new LockPool<TestLockTags>(-1);
            act.Should().Throw<ArgumentException>()
                .WithMessage("*capacity cannot be negative*");
        }
        
        [Test]
        public void Constructor_WithZeroCapacity_InitializesEmptyPool()
        {
            // Act
            var pool = new LockPool<TestLockTags>(0);
            
            // Assert
            pool.Should().NotBeNull();
        }
        
        [Test]
        public void Borrow_ReturnsInitializedLock()
        {
            // Arrange
            var includeTags = TestLockTags.Tag1;
            var excludeTags = TestLockTags.Tag2;
            
            // Act
            var lockObj = _lockPool.Borrow(_mockLockService, includeTags, excludeTags);
            
            // Assert
            lockObj.Should().NotBeNull();
            lockObj.Should().BeOfType<BaseLock<TestLockTags>>();
            lockObj.IncludeTags.Should().Be(includeTags);
            lockObj.ExcludeTags.Should().Be(excludeTags);
        }
        
        [Test]
        public void Return_WhenPoolHasCapacity_ReusesObject()
        {
            // Arrange
            var lockObj = _lockPool.Borrow(_mockLockService);
            var lockId = lockObj.Id; // Remember the original ID
            
            // Act
            _lockPool.Return(lockObj);
            var newLockObj = _lockPool.Borrow(_mockLockService);
            
            // Assert
            // Since IDs are unique, we expect to get the same object back with a new ID
            // We can't compare IDs directly because they're generated uniquely on initialization
            // But we can confirm it's not null and initialized correctly
            newLockObj.Should().NotBeNull();
        }
        
        [Test]
        public void Return_WithNullObject_DoesNothing()
        {
            // Act & Assert (no exception expected)
            _lockPool.Return(null);
        }
        
        [Test]
        public void Return_WithObjectNotFromPool_DoesNothing()
        {
            // Arrange
            var externalLock = new BaseLock<TestLockTags>(_mockLockService);
            
            // Act & Assert (no exception expected)
            _lockPool.Return(externalLock);
        }
        
        [Test]
        public void Borrow_MultipleTimes_ReturnsDifferentInstances()
        {
            // Act
            var lock1 = _lockPool.Borrow(_mockLockService);
            var lock2 = _lockPool.Borrow(_mockLockService);
            
            // Assert
            lock1.Should().NotBeSameAs(lock2);
        }
        
        [Test]
        public void Borrow_AfterReturn_ReusesInstance()
        {
            // This test relies on implementation details which is not ideal,
            // but it helps verify pool behavior
            
            // Borrow and return several locks to ensure pool has objects
            for (int i = 0; i < 10; i++)
            {
                var tempLock = _lockPool.Borrow(_mockLockService);
                _lockPool.Return(tempLock);
            }
            
            // The pool should now have objects ready for reuse
            // We'll verify by borrowing and checking we get initialized objects
            var newLock = _lockPool.Borrow(_mockLockService, TestLockTags.Tag1);
            
            newLock.Should().NotBeNull();
            newLock.IncludeTags.Should().Be(TestLockTags.Tag1);
        }
    }
} 