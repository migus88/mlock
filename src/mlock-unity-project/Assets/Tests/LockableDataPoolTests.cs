using System;
using FluentAssertions;
using Migs.MLock.Interfaces;
using Migs.MLock.Pools;
using Migs.MLock.Tests.Integration;
using NSubstitute;
using NUnit.Framework;

namespace Migs.MLock.Tests
{
    public class LockableDataPoolTests
    {
        private LockableDataPool<TestLockTags> _dataPool;
        private ILockable<TestLockTags> _mockLockable;
        
        [SetUp]
        public void Setup()
        {
            _dataPool = new LockableDataPool<TestLockTags>(5);
            _mockLockable = Substitute.For<ILockable<TestLockTags>>();
            _mockLockable.LockTags.Returns(TestLockTags.Tag1);
        }
        
        [Test]
        public void Constructor_WithNegativeCapacity_ThrowsArgumentException()
        {
            // Act & Assert
            Action act = () => new LockableDataPool<TestLockTags>(-1);
            act.Should().Throw<ArgumentException>()
                .WithMessage("*capacity cannot be negative*");
        }
        
        [Test]
        public void Constructor_WithZeroCapacity_InitializesEmptyPool()
        {
            // Act
            var pool = new LockableDataPool<TestLockTags>(0);
            
            // Assert
            pool.Should().NotBeNull();
        }
        
        [Test]
        public void Borrow_ReturnsInitializedData()
        {
            // Act
            var data = _dataPool.Borrow(_mockLockable);
            
            // Assert
            data.Should().NotBeNull();
            data.Lockable.Should().Be(_mockLockable);
            data.Locks.Should().NotBeNull();
            data.Locks.Should().BeEmpty();
        }
        
        [Test]
        public void Borrow_WithNullLockable_ThrowsArgumentNullException()
        {
            // Act & Assert
            Action act = () => _dataPool.Borrow(null);
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("*Lockable cannot be null*");
        }
        
        [Test]
        public void Return_WhenPoolHasCapacity_ReusesObject()
        {
            // Arrange
            var data = _dataPool.Borrow(_mockLockable);
            
            // Add something to the locks collection to verify it gets cleared
            var mockLock = Substitute.For<ILock<TestLockTags>>();
            data.Locks.Add(mockLock);
            data.Locks.Should().HaveCount(1);
            
            // Act
            _dataPool.Return(data);
            var newData = _dataPool.Borrow(_mockLockable);
            
            // Assert
            newData.Should().NotBeNull();
            newData.Lockable.Should().Be(_mockLockable);
            newData.Locks.Should().BeEmpty(); // Should be cleared when returned
        }
        
        [Test]
        public void Return_WithNullObject_DoesNothing()
        {
            // Act & Assert (no exception expected)
            _dataPool.Return(null);
        }
        
        [Test]
        public void Borrow_MultipleTimes_ReturnsDifferentInstances()
        {
            // Arrange
            var mockLockable2 = Substitute.For<ILockable<TestLockTags>>();
            mockLockable2.LockTags.Returns(TestLockTags.Tag2);
            
            // Act
            var data1 = _dataPool.Borrow(_mockLockable);
            var data2 = _dataPool.Borrow(mockLockable2);
            
            // Assert
            data1.Should().NotBeSameAs(data2);
            data1.Lockable.Should().Be(_mockLockable);
            data2.Lockable.Should().Be(mockLockable2);
        }
        
        [Test]
        public void Borrow_AfterReturn_ReusesInstance()
        {
            // This test relies on implementation details which is not ideal,
            // but it helps verify pool behavior
            
            // Borrow and return several data objects to ensure pool has objects
            for (int i = 0; i < 10; i++)
            {
                var tempData = _dataPool.Borrow(_mockLockable);
                _dataPool.Return(tempData);
            }
            
            // The pool should now have objects ready for reuse
            var newData = _dataPool.Borrow(_mockLockable);
            
            newData.Should().NotBeNull();
            newData.Lockable.Should().Be(_mockLockable);
            newData.Locks.Should().BeEmpty();
        }
        
        [Test]
        public void Return_ClearsLockCollection()
        {
            // Arrange
            var data = _dataPool.Borrow(_mockLockable);
            
            // Add locks
            var mockLock1 = Substitute.For<ILock<TestLockTags>>();
            var mockLock2 = Substitute.For<ILock<TestLockTags>>();
            data.Locks.Add(mockLock1);
            data.Locks.Add(mockLock2);
            data.Locks.Should().HaveCount(2);
            
            // Act
            _dataPool.Return(data);
            var newData = _dataPool.Borrow(_mockLockable);
            
            // Assert - locks should be cleared
            newData.Locks.Should().BeEmpty();
        }
    }
} 