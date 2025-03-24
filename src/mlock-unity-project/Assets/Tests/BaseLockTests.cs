using System;
using FluentAssertions;
using Migs.MLock.Interfaces;
using Migs.MLock.Tests.Integration;
using NSubstitute;
using NUnit.Framework;

namespace Migs.MLock.Tests
{
    public class BaseLockTests
    {
        private ILockService<TestLockTags> _mockLockService;
        private ILockPool<TestLockTags> _mockLockPool;
        
        [SetUp]
        public void Setup()
        {
            _mockLockService = Substitute.For<ILockService<TestLockTags>>();
            _mockLockPool = Substitute.For<ILockPool<TestLockTags>>();
        }
        
        [Test]
        public void Constructor_WithValidParameters_InitializesProperties()
        {
            // Arrange
            var includeTags = TestLockTags.Tag1;
            var excludeTags = TestLockTags.Tag2;
            
            // Act
            var baseLock = new BaseLock<TestLockTags>(_mockLockService, includeTags, excludeTags, _mockLockPool);
            
            // Assert
            baseLock.Id.Should().BeGreaterThan(0);
            baseLock.IncludeTags.Should().Be(includeTags);
            baseLock.ExcludeTags.Should().Be(excludeTags);
        }
        
        [Test]
        public void Constructor_WithNullService_ThrowsArgumentNullException()
        {
            // Act & Assert
            Action act = () => new BaseLock<TestLockTags>(null);
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("*Lock service cannot be null*");
        }
        
        [Test]
        public void Initialize_WithValidParameters_SetsProperties()
        {
            // Arrange
            var baseLock = new BaseLock<TestLockTags>();
            var includeTags = TestLockTags.Tag1;
            var excludeTags = TestLockTags.Tag2;
            
            // Act
            baseLock.Initialize(_mockLockService, includeTags, excludeTags, _mockLockPool);
            
            // Assert
            baseLock.Id.Should().BeGreaterThan(0);
            baseLock.IncludeTags.Should().Be(includeTags);
            baseLock.ExcludeTags.Should().Be(excludeTags);
        }
        
        [Test]
        public void Dispose_CallsUnlockAndReturnsToPool()
        {
            // Arrange
            var baseLock = new BaseLock<TestLockTags>(_mockLockService, TestLockTags.Tag1, default, _mockLockPool);
            
            // Act
            baseLock.Dispose();
            
            // Assert
            _mockLockService.Received(1).TryUnlocking(baseLock);
            _mockLockPool.Received(1).Return(baseLock);
        }
        
        [Test]
        public void Dispose_WithNullPool_OnlyCallsUnlock()
        {
            // Arrange
            var baseLock = new BaseLock<TestLockTags>(_mockLockService);
            
            // Act
            baseLock.Dispose();
            
            // Assert
            _mockLockService.Received(1).TryUnlocking(baseLock);
            // No pool to receive Return call
        }
        
        [Test]
        public void ToString_ReturnsExpectedFormat()
        {
            // Arrange
            var baseLock = new BaseLock<TestLockTags>(_mockLockService);
            var expectedString = $"{nameof(BaseLock<TestLockTags>)}||{baseLock.Id}";
            
            // Act
            var result = baseLock.ToString();
            
            // Assert
            result.Should().Be(expectedString);
        }
        
        [Test]
        public void EachLock_GetsUniqueId()
        {
            // Arrange & Act
            var lock1 = new BaseLock<TestLockTags>(_mockLockService);
            var lock2 = new BaseLock<TestLockTags>(_mockLockService);
            
            // Assert
            lock1.Id.Should().NotBe(lock2.Id);
        }
    }
} 