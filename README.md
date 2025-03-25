# MLock

[![Unity Version](https://img.shields.io/badge/Unity-2022.3+-blue.svg)](https://unity.com/releases/editor/whats-new/2022.3.0)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Version](https://img.shields.io/badge/Version-1.0.0-blue.svg)](src/mlock-unity-project/Packages/MLock/package.json)

A flexible and efficient lock system for Unity that manages access to gameplay elements based on configurable tags.

## Overview

MLock is a powerful lock management system designed specifically for Unity that provides a structured way to control access to game objects and features. It uses a tag-based approach to determine which objects should be locked or unlocked at any given time, making it ideal for managing game state, feature access, UI interactions, and more.

## Features

- **Tag-based Locking**: Lock and unlock objects based on customizable enum tags
- **Efficient Object Pooling**: Built-in object pooling for optimal performance
- **Flexible API**: Simple but powerful interface for integration into any project
- **Inclusion/Exclusion Logic**: Lock specific tags or lock everything except specific tags
- **Lightweight**: Minimal overhead with a focus on performance
- **Generic Implementation**: Works with any custom enum type for maximum flexibility
- **[Debug Windows](#debug-windows)**: Built-in tools to monitor and control locks in real-time

## Installation

### Using Unity Package Manager (Recommended)

1. Open your Unity project
2. Go to Window > Package Manager
3. Click the + button > Add package from git URL
4. Enter `https://github.com/migus88/sandland-lock-system.git?path=/src/mlock-unity-project/Packages/MLock`
5. Click Add

### Manual Installation

1. Download this repository
2. Copy the `MLock` folder to your Unity project's `Packages` directory

## Quick Start

1. Define your lock tags enum (power-of-two values required as well as a zero state):

```csharp
[System.Flags] // Optional
public enum GameplayFeatures
{
    None = 0, // Required
    Movement = 1 << 0,
    Inventory = 1 << 1,
    Dialog = 1 << 2,
    Combat = 1 << 3,
    // Add more as needed
}
```

2. Create a lock service:

```csharp
using Migs.MLock;
using Migs.MLock.Interfaces;

// Create service instance (should be shared across a domain)
private ILockService<GameplayFeatures> _lockService;

void Awake()
{
    _lockService = new BaseLockService<GameplayFeatures>();
}
```

3. Implement the ILockable interface on objects that can be locked:

```csharp
using Migs.MLock.Interfaces;
using UnityEngine;

public class PlayerController : MonoBehaviour, ILockable<GameplayFeatures>
{
    public GameplayFeatures LockTags => GameplayFeatures.Movement;
    
    private bool _isLocked = false;

    public void Start()
    {
        // Don't actually do that - use a proper DI framework
        GameManager.Instance.LockService.Subscribe(this);
    }

    public void OnDestroy()
    {
        GameManager.Instance.LockService.Unsubscribe(this);
    }
    
    public void HandleLocking()
    {
        _isLocked = true;
        // Disable movement logic here
    }
    
    public void HandleUnlocking()
    {
        _isLocked = false;
        // Enable movement logic here
    }
    
    void Update()
    {
        if (!_isLocked)
        {
            // Movement logic here
        }
    }
}
```

4. Create and use locks:

```csharp
// Lock specific features
ILock<GameplayFeatures> movementLock = _lockService.Lock(GameplayFeatures.Movement);

// Lock everything except specific features
ILock<GameplayFeatures> lockAllButDialog = _lockService.LockAllExcept(GameplayFeatures.Dialog);

// Lock everything
ILock<GameplayFeatures> lockAll = _lockService.LockAll();

// Unlock by disposing the lock
movementLock.Dispose();
```

## Examples

The repository includes a Car Example that demonstrates the MLock system in action. The example shows a simple car interface with menu systems where different UI elements need to be locked based on the current context. The same functionality is implemented using two different UI approaches:

### Available Examples:

- [Shared Components](src/mlock-unity-project/Assets/Examples/Shared)
- [Car with Unity Canvas UI](src/mlock-unity-project/Assets/Examples/Car_Canvas)
- [Car with UI Toolkit (UI Elements)](src/mlock-unity-project/Assets/Examples/Car_UiElements)

## Advanced Usage

### Using Locks with Using Statements

1. With predefined scope
```csharp
// Locks are automatically disposed at the end of the block
using (_lockService.Lock(GameplayFeatures.Combat))
{
    // Combat is locked within this block
    // Run combat sequence or cutscene
}
// Combat is automatically unlocked when exiting the block
```
2. Until the end of the current scope
```csharp
// Locks are automatically disposed at the end of the method
public void SomeComplexMethod()
{
    using var combatLock = _lockService.Lock(GameplayFeatures.Combat);

    // Do a lot of things here
    // ...
} // Combat is automatically unlocked when exiting the method
```

### Check Lock Status

```csharp
if (_lockService.IsLocked(playerController))
{
    // Player is currently locked
}
```

## Debug Windows

MLock comes with built-in debug windows that provide real-time monitoring and control of your lock system directly within the Unity Editor.

### Available Debug Windows

- **Locks Debug Window**: Shows all active locks, affected objects, and lock details
- **Services Debug Window**: Displays registered lock services and their status

### Opening Debug Windows

1. In Unity, go to `Window > MLock > Locks Debug` or `Window > MLock > Services Debug`

### Registering Services for Debugging

For the debug windows to work, you need to register your lock services:

```csharp
// Using extension method (recommended)
var lockService = new BaseLockService<MyLockTags>().WithDebug();

// Unregister when no longer needed
lockService.WithoutDebug();
```

### Debug Window Features

- **Real-time monitoring** of all active locks in your game
- **Search functionality** to filter locks by lockables, tags, or other criteria
- **Unlock buttons** to release specific locks during gameplay
- **Unlock All** to quickly reset all locks in the system
- **Auto-refresh** to keep the display updated with the latest information

### Debug Window Benefits

- **Troubleshooting**: Quickly identify which locks are active when unexpected behavior occurs
- **Development**: Test locking/unlocking features without modifying code
- **QA**: Verify lock system behavior and relationships between game objects
- **Performance Monitoring**: See how many locks are active at any given time

Note: Debug windows only work in the Unity Editor and have no impact on your game builds.

## Architecture

The MLock system is built around these core components:

- **ILockService**: Manages lockable objects and locks
- **ILock**: Represents a lock that can be applied to lockable objects
- **ILockable**: Interface for objects that can be locked
- **Object Pools**: Efficient reuse of lock instances for better performance

## Best Practices

- Use power-of-two values for enum tags to support bitwise operations
- Dispose locks when they're no longer needed
- Consider using a dependency injection system to provide lock services
- Group related features under single tags for easier management

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Contact

Yuri Sokolov - [GitHub](https://github.com/migus88) 