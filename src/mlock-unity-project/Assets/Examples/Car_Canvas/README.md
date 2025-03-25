# Car Example with Unity Canvas UI

This example demonstrates how to use the `MLock` system in a simple application with Unity's Canvas UI (UGUI) system.

## Overview

The example showcases a simple car scene with a menu system that uses `MLock` to manage UI interaction and prevent unwanted input while different menus are open.

## Key Components

- **CarExample.cs**: Main controller script that manages the car UI and demonstrates `MLock` usage
- **Canvas Prefab**: Contains all the UI elements arranged in a traditional Unity Canvas hierarchy
- **Lockable Controls**: Custom UI components that implement the ILockable interface

## How It Works

1. The example uses `MLock` to manage different UI states:
   - When opening the main menu, it locks player input and game HUD
   - When opening the language tooltip, it locks the main menu
   - The "Temp Lock" button demonstrates locking all UI temporarily

## How Lockables Are Created

Lockables are implemented by extending standard Unity UI components and implementing the `ILockable<T>` interface:

```csharp
public class LockableButton : Button, ILockable<CarLockTags>
{
    [field:SerializeField] public CarLockTags LockTags { get; set; }

    protected override void Awake()
    {
        base.Awake();
        CarLockService.Instance.Subscribe(this);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        CarLockService.Instance?.Unsubscribe(this);
    }

    public void HandleLocking()
    {
        interactable = false;
    }

    public void HandleUnlocking()
    {
        interactable = true;
    }
}
```

Key aspects:
1. Inherit from a Unity UI component (Button, Toggle, etc.)
2. Implement the `ILockable<CarLockTags>` interface
3. Subscribe to the lock service in Awake()
4. Unsubscribe in OnDestroy()
5. Implement lock/unlock behavior in HandleLocking/HandleUnlocking methods

## Disclaimer

This example was created with a focus on demonstrating `MLock` functionality, not on performance or coding best practices. 

**NOT RECOMMENDED FOR PRODUCTION:**
- The use of Singletons (`CarLockService.Instance`)
- Frequent use of `GetComponent` and similar methods
- Other simplifications made for clarity

In a real application, you should consider using dependency injection or other appropriate patterns for managing services and references. 