using FluentAssertions;
using Migs.MLock.Tests.Integration;
using NUnit.Framework;

namespace Migs.MLock.Tests
{
    public class MLockIntegrationTests
    {
        private BaseLockService<GameFeature> _lockService;
        private GameFeatureController _movementController;
        private GameFeatureController _inventoryController;
        private GameFeatureController _combatController;
        private GameFeatureController _dialogController;
        private GameFeatureController _menuController;
        
        [SetUp]
        public void Setup()
        {
            _lockService = new BaseLockService<GameFeature>();
            
            _movementController = new GameFeatureController(GameFeature.Movement);
            _inventoryController = new GameFeatureController(GameFeature.Inventory);
            _combatController = new GameFeatureController(GameFeature.Combat);
            _dialogController = new GameFeatureController(GameFeature.Dialog);
            _menuController = new GameFeatureController(GameFeature.Menu);
            
            _lockService.Subscribe(_movementController);
            _lockService.Subscribe(_inventoryController);
            _lockService.Subscribe(_combatController);
            _lockService.Subscribe(_dialogController);
            _lockService.Subscribe(_menuController);
        }
        
        [Test]
        public void BaseLockService_CorrectlyLocksAllFeatures()
        {
            // Act
            using (var lockObj = _lockService.LockAll())
            {
                // Assert
                _movementController.IsLocked.Should().BeTrue();
                _inventoryController.IsLocked.Should().BeTrue();
                _combatController.IsLocked.Should().BeTrue();
                _dialogController.IsLocked.Should().BeTrue();
                _menuController.IsLocked.Should().BeTrue();
            }
            
            // After Dispose(), everything should be unlocked
            _movementController.IsLocked.Should().BeFalse();
            _inventoryController.IsLocked.Should().BeFalse();
            _combatController.IsLocked.Should().BeFalse();
            _dialogController.IsLocked.Should().BeFalse();
            _menuController.IsLocked.Should().BeFalse();
        }
        
        [Test]
        public void BaseLockService_CorrectlyLocksSpecificFeatures()
        {
            // Act - Lock only gameplay features (Movement, Combat)
            using (var lockObj = _lockService.Lock(GameFeature.Movement | GameFeature.Combat))
            {
                // Assert
                _movementController.IsLocked.Should().BeTrue();
                _inventoryController.IsLocked.Should().BeFalse();
                _combatController.IsLocked.Should().BeTrue();
                _dialogController.IsLocked.Should().BeFalse();
                _menuController.IsLocked.Should().BeFalse();
            }
            
            // After Dispose(), everything should be unlocked
            _movementController.IsLocked.Should().BeFalse();
            _combatController.IsLocked.Should().BeFalse();
        }
        
        [Test]
        public void BaseLockService_CorrectlyLocksAllExceptSpecificFeatures()
        {
            // Act - Lock everything except Menu (so player can pause)
            using (var lockObj = _lockService.LockAllExcept(GameFeature.Menu))
            {
                // Assert
                _movementController.IsLocked.Should().BeTrue();
                _inventoryController.IsLocked.Should().BeTrue();
                _combatController.IsLocked.Should().BeTrue();
                _dialogController.IsLocked.Should().BeTrue();
                _menuController.IsLocked.Should().BeFalse(); // Menu should remain unlocked
            }
            
            // After Dispose(), everything should be unlocked
            _movementController.IsLocked.Should().BeFalse();
            _inventoryController.IsLocked.Should().BeFalse();
            _combatController.IsLocked.Should().BeFalse();
            _dialogController.IsLocked.Should().BeFalse();
            _menuController.IsLocked.Should().BeFalse();
        }
        
        [Test]
        public void BaseLockService_CorrectlyTracksPriorityLocks()
        {
            // Act - First lock gameplay features
            using (var gameplayLock = _lockService.Lock(GameFeature.Movement | GameFeature.Combat))
            {
                _movementController.IsLocked.Should().BeTrue();
                _combatController.IsLocked.Should().BeTrue();
                
                // Then lock everything with another lock
                using (var allLock = _lockService.LockAll())
                {
                    // All should be locked
                    _movementController.IsLocked.Should().BeTrue();
                    _inventoryController.IsLocked.Should().BeTrue();
                    _combatController.IsLocked.Should().BeTrue();
                    _dialogController.IsLocked.Should().BeTrue();
                    _menuController.IsLocked.Should().BeTrue();
                }
                
                // After inner lock disposal, original locks should remain
                _movementController.IsLocked.Should().BeTrue();
                _inventoryController.IsLocked.Should().BeFalse();
                _combatController.IsLocked.Should().BeTrue();
                _dialogController.IsLocked.Should().BeFalse();
                _menuController.IsLocked.Should().BeFalse();
            }
            
            // After all locks disposed, nothing should be locked
            _movementController.IsLocked.Should().BeFalse();
            _inventoryController.IsLocked.Should().BeFalse();
            _combatController.IsLocked.Should().BeFalse();
            _dialogController.IsLocked.Should().BeFalse();
            _menuController.IsLocked.Should().BeFalse();
        }
        
        [Test]
        public void BaseLockService_CorrectlyHandlesDynamicSubscription()
        {
            // Act - Create a lock first
            using (var lockObj = _lockService.Lock(GameFeature.Movement))
            {
                // Initial state
                _movementController.IsLocked.Should().BeTrue();
                
                // Create a new controller and subscribe it
                var newMovementController = new GameFeatureController(GameFeature.Movement);
                _lockService.Subscribe(newMovementController);
                
                // It should immediately be locked by the existing lock
                newMovementController.IsLocked.Should().BeTrue();
                
                // Unsubscribe it while lock is active
                _lockService.Unsubscribe(newMovementController);
                
                // It should be unlocked when unsubscribed
                newMovementController.IsLocked.Should().BeFalse();
                
                // Original controller should still be locked
                _movementController.IsLocked.Should().BeTrue();
            }
            
            // After lock disposed, original controller should be unlocked
            _movementController.IsLocked.Should().BeFalse();
        }
        
        [Test]
        public void BaseLockService_CorrectlyCombinesMultipleLocks()
        {
            // Create two controllers with combined tags
            var multiFeatureController = new GameFeatureController(GameFeature.Movement | GameFeature.Combat);
            _lockService.Subscribe(multiFeatureController);
            
            // Lock only Movement
            using (var movementLock = _lockService.Lock(GameFeature.Movement))
            {
                // Controller with Movement tag should be locked
                _movementController.IsLocked.Should().BeTrue();
                
                // Controller with Movement+Combat should also be locked
                multiFeatureController.IsLocked.Should().BeTrue();
                
                // Combat controller should not be locked yet
                _combatController.IsLocked.Should().BeFalse();
                
                // Now add a Combat lock
                using (var combatLock = _lockService.Lock(GameFeature.Combat))
                {
                    // Now both specific controllers should be locked
                    _movementController.IsLocked.Should().BeTrue();
                    _combatController.IsLocked.Should().BeTrue();
                    
                    // And the multi-feature controller should still be locked
                    multiFeatureController.IsLocked.Should().BeTrue();
                }
                
                // After combat lock is disposed
                _movementController.IsLocked.Should().BeTrue(); // Still locked by movement lock
                _combatController.IsLocked.Should().BeFalse(); // No longer locked
                multiFeatureController.IsLocked.Should().BeTrue(); // Still locked by movement lock
            }
            
            // After all locks disposed
            _movementController.IsLocked.Should().BeFalse();
            _combatController.IsLocked.Should().BeFalse();
            multiFeatureController.IsLocked.Should().BeFalse();
        }
    }
} 